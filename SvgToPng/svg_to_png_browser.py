#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
SVG to PNG Converter using Headless Browser

This script converts SVG files to PNG format using Playwright's headless browser
for the highest quality rendering that preserves all text spacing and visual elements.
"""

import asyncio
import os
import sys
from pathlib import Path
from typing import List
from playwright.async_api import async_playwright


def get_svg_files(source_dir: Path) -> List[Path]:
    """Get all SVG files from the source directory."""
    if not source_dir.exists():
        raise FileNotFoundError(f"Source directory not found: {source_dir}")
    
    svg_files = list(source_dir.glob("*.svg"))
    if not svg_files:
        print(f"No SVG files found in {source_dir}")
        return []
    
    return svg_files


async def convert_svg_to_png_browser(svg_path: Path, png_path: Path, browser, page) -> bool:
    """Convert a single SVG file to PNG format using headless browser."""
    try:
        # Read SVG content
        with open(svg_path, 'r', encoding='utf-8') as f:
            svg_content = f.read()
        
        # Create HTML wrapper for proper SVG rendering
        html_content = f"""
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset="UTF-8">
            <style>
                body {{
                    margin: 0;
                    padding: 20px;
                    background: white;
                    font-family: "trebuchet ms", verdana, arial, sans-serif;
                }}
                svg {{
                    display: block;
                    margin: 0 auto;
                }}
            </style>
        </head>
        <body>
            {svg_content}
        </body>
        </html>
        """
        
        # Set content and wait for fonts to load properly
        await page.set_content(html_content)
        await page.wait_for_load_state('networkidle')
        
        # Get SVG element and take screenshot
        svg_element = await page.query_selector('svg')
        if not svg_element:
            print(f"Error: No SVG element found in {svg_path.name}")
            return False
        
        # Take high-quality screenshot without clip parameter
        await svg_element.screenshot(path=str(png_path))
        return True
        
    except Exception as e:
        print(f"Error converting {svg_path.name}: {e}")
        return False


async def convert_all_svgs(svg_files: List[Path], destination_dir: Path):
    """Convert all SVG files to PNG format."""
    total_files = len(svg_files)
    successful_conversions = 0
    failed_conversions = 0
    
    print(f"Found {total_files} SVG files to convert using headless browser...")
    print("-" * 50)
    
    async with async_playwright() as p:
        # Launch browser once and reuse it
        browser = await p.chromium.launch(headless=True)
        page = await browser.new_page()
        
        for i, svg_file in enumerate(svg_files, 1):
            # Create PNG filename
            png_filename = svg_file.stem + ".png"
            png_path = destination_dir / png_filename
            
            print(f"[{i}/{total_files}] Converting {svg_file.name} -> {png_filename}")
            
            # Convert the file
            if await convert_svg_to_png_browser(svg_file, png_path, browser, page):
                successful_conversions += 1
                print(f"  ✓ Success")
            else:
                failed_conversions += 1
                print(f"  ✗ Failed")
        
        await browser.close()
    
    # Print summary
    print("-" * 50)
    print(f"Conversion complete!")
    print(f"  Successful: {successful_conversions}")
    print(f"  Failed: {failed_conversions}")
    print(f"  Total: {total_files}")
    
    return failed_conversions == 0


async def main():
    """Main conversion function."""
    # Define source and destination directories
    source_dir = Path("source")
    destination_dir = Path("destination")
    
    # Create destination directory if it doesn't exist
    destination_dir.mkdir(exist_ok=True)
    
    # Get all SVG files
    try:
        svg_files = get_svg_files(source_dir)
    except FileNotFoundError as e:
        print(f"Error: {e}")
        sys.exit(1)
    
    if not svg_files:
        sys.exit(0)
    
    # Convert all files
    success = await convert_all_svgs(svg_files, destination_dir)
    
    if not success:
        sys.exit(1)


if __name__ == "__main__":
    asyncio.run(main())