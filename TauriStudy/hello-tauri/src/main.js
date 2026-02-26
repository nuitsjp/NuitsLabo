const { getCurrentWindow } = window.__TAURI__.window;

document.addEventListener("DOMContentLoaded", () => {
  const appWindow = getCurrentWindow();

  function setTransparent(transparent) {
    document.body.classList.toggle("transparent", transparent);
    document.documentElement.style.backgroundColor = transparent
      ? "transparent"
      : "";
    appWindow.setDecorations(!transparent);
  }

  appWindow.onFocusChanged(({ payload: focused }) => {
    setTransparent(!focused);
  });
});
