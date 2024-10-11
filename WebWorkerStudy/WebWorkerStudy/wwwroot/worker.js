/* worker.js (Web Worker用スクリプト) */
self.onmessage = function(event) {
  const n = event.data;
  let sum = 0;
  for (let i = 1; i <= n; i++) {
    sum += i;
  }
  // 計算結果をメインスレッドに送信
  self.postMessage(sum);
};