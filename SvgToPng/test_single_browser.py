#!/usr/bin/env python3
import asyncio
from pathlib import Path
from playwright.async_api import async_playwright

async def test_single_conversion():
    svg_path = Path("source/architecture_mermaid_1_34c93a8f.svg")
    png_path = Path("destination/architecture_mermaid_1_34c93a8f_browser.png")
    
    async with async_playwright() as p:
        browser = await p.chromium.launch(headless=True)
        page = await browser.new_page()
        
        # Read SVG content
        with open(svg_path, 'r', encoding='utf-8') as f:
            svg_content = f.read()
        
        # Create HTML wrapper
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
        
        await page.set_content(html_content)
        await page.wait_for_load_state('networkidle')
        
        # Take screenshot
        svg_element = await page.query_selector('svg')
        if svg_element:
            await svg_element.screenshot(path=str(png_path), type='png')
            print("✓ Conversion successful")
        else:
            print("✗ SVG element not found")
        
        await browser.close()

if __name__ == "__main__":
    asyncio.run(test_single_conversion())