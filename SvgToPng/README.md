# SVG to PNG Converter

A Python project that converts SVG files to PNG format using cairosvg.

## Features

- Batch conversion of all SVG files in the `source/` folder
- PNG output saved to `destination/` folder
- Progress tracking and error handling
- Maintains original filenames with PNG extension
- Uses modern Python dependency management with uv

## Requirements

- Python 3.8+
- uv (for dependency management)

## Installation

1. Install uv if you haven't already:
   ```bash
   curl -LsSf https://astral.sh/uv/install.sh | sh
   ```

2. Install project dependencies:
   ```bash
   uv sync
   ```

## Usage

1. Place your SVG files in the `source/` folder
2. Run the conversion script:
   ```bash
   uv run svg_to_png.py
   ```

The converted PNG files will be saved in the `destination/` folder.

## Project Structure

```
SvgToPng/
   source/              # Place SVG files here
   destination/         # PNG files will be saved here
   svg_to_png.py       # Main conversion script
   pyproject.toml      # Project configuration
   README.md           # This file
```

## Example Output

```
Found 24 SVG files to convert...
--------------------------------------------------
[1/24] Converting architecture_mermaid_0_07a67020.svg -> architecture_mermaid_0_07a67020.png
   Success
[2/24] Converting architecture_mermaid_1_34c93a8f.svg -> architecture_mermaid_1_34c93a8f.png
   Success
...
--------------------------------------------------
Conversion complete!
  Successful: 24
  Failed: 0
  Total: 24
```