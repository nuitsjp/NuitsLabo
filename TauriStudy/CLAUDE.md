# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Tauri v2 desktop application ("hello-tauri") — a learning project demonstrating hybrid JavaScript/Rust architecture with transparent window management and focus-aware UI.

## Build & Development Commands

All commands run from `hello-tauri/` directory:

```bash
# Install dependencies
npm install

# Development (hot-reload)
npm run tauri dev

# Production build (outputs .exe, .msi, .nsis to src-tauri/target/release/)
npm run tauri build
```

Rust checks from `hello-tauri/src-tauri/`:
```bash
cargo check
cargo clippy
cargo test
```

## Architecture

```
hello-tauri/
├── src/              # Frontend: vanilla HTML/CSS/JS (no framework)
│   ├── main.js       # Window management via __TAURI__.window API
│   └── styles.css    # Dark mode, transparency, drag region support
├── src-tauri/        # Backend: Rust
│   ├── src/lib.rs    # Tauri app builder with plugin registration
│   ├── src/main.rs   # Entry point (delegates to lib::run())
│   ├── tauri.conf.json       # App config (window, bundle, security)
│   └── capabilities/default.json  # Tauri permission declarations
└── package.json      # npm scripts delegate to Tauri CLI
```

**Frontend ↔ Backend**: The app uses `withGlobalTauri: true` to expose Tauri APIs on `window.__TAURI__`. The frontend calls window management APIs directly (setDecorations, setAlwaysOnTop, onFocusChanged). No custom Tauri commands are defined yet.

**Key behavior**: Window becomes transparent and undecorated when losing focus, restoring decorations on regain.

## Tech Stack

- **Frontend**: Vanilla JS (ES6+), no bundler — `src/` served directly as `frontendDist`
- **Backend**: Rust 2021 edition, Tauri 2.x, tauri-plugin-opener
- **Package manager**: pnpm (lockfile v9)
- **Build**: Tauri CLI v2 via npm scripts

## Prerequisites

- Node.js v18+
- Rust (rustup)
- Visual Studio "Desktop development with C++" workload (Windows)

## Conventions

- Commit messages follow Conventional Commits (`feat:`, `fix:`, etc.)
- CSP is disabled (`"csp": null`) — this is a learning project
- Tauri permissions are declared in `src-tauri/capabilities/default.json`
