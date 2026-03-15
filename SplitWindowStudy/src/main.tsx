import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { ParentWindow } from "./ParentWindow";
import { ChildWindow } from "./ChildWindow";

const isChild = new URLSearchParams(window.location.search).get("mode") === "child";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    {isChild ? <ChildWindow /> : <ParentWindow />}
  </StrictMode>
);
