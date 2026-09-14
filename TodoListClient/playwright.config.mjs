import { defineConfig, devices } from "@playwright/test";

export default defineConfig({
  testDir: "./e2e",
  fullyParallel: false,
  workers: 1,
  forbidOnly: !!process.env.CI,
  retries: process.env.CI ? 2 : 0,
  reporter: process.env.CI ? "github" : "list",
  use: {
    baseURL: "http://localhost:4200",
    trace: "on-first-retry",
  },
  projects: [
    {
      name: "chromium",
      use: { ...devices["Desktop Chrome"] },
    },
  ],
  webServer: [
    {
      command: "dotnet run --project ../TodoListAPI",
      url: "http://localhost:5000/api/v1/todo-items",
      env: {
        ASPNETCORE_ENVIRONMENT: "Development",
        ASPNETCORE_URLS: "http://localhost:5000",
      },
      reuseExistingServer: !process.env.CI,
      timeout: 120 * 1000,
      stdout: "pipe",
      stderr: "pipe",
    },
    {
      command: "npm start",
      url: "http://localhost:4200",
      reuseExistingServer: !process.env.CI,
      timeout: 120 * 1000,
      stdout: "pipe",
      stderr: "pipe",
    },
  ],
});
