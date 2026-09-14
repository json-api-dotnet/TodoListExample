import { test, expect } from '@playwright/test';

test.describe('Todo List Application', () => {
  test('Application loads at http://localhost:4200 and displays the sign-in form', async ({
    page,
  }) => {
    await page.goto('/');
    await expect(page).toHaveURL(/\/login/);
    await expect(page.locator('h1')).toHaveText('Please sign in');
    await expect(page.locator('[data-test-username]')).toBeVisible();
    await expect(page.locator('[data-test-password]')).toBeVisible();
    await expect(page.locator('[data-test-submit]')).toBeVisible();
  });

  test('Attempting to log in with invalid credentials displays an "Authentication failed" alert', async ({
    page,
  }) => {
    await page.goto('/login');
    await page.locator('[data-test-username]').fill('wrong-user');
    await page.locator('[data-test-password]').fill('wrong-password');
    await page.locator('[data-test-submit]').click();

    const alertMessage = page.locator('.ember-notify .message');
    await expect(alertMessage).toBeVisible();
    await expect(alertMessage).toHaveText('Authentication failed');
    await expect(page).toHaveURL(/\/login/);
  });

  test('Logging in as guest displays the todo list containing "owned-by-guest"', async ({
    page,
  }) => {
    await page.goto('/login');
    await page.locator('[data-test-username]').fill('guest');
    await page.locator('[data-test-password]').fill('Guest1!');
    await page.locator('[data-test-submit]').click();

    await expect(page).toHaveURL(/\/s\/todo-items/);
    await expect(page.locator('h1')).toHaveText('Todo Items');
    await expect(page.locator('[data-test-description]')).toContainText([
      'owned-by-guest',
    ]);
    await expect(page.locator('tbody')).not.toContainText('owned-by-john');
  });

  test('Logging in as john displays the todo list containing "owned-by-john"', async ({
    page,
  }) => {
    await page.goto('/login');
    await page.locator('[data-test-username]').fill('john');
    await page.locator('[data-test-password]').fill('P@ssw0rd!');
    await page.locator('[data-test-submit]').click();

    await expect(page).toHaveURL(/\/s\/todo-items/);
    await expect(page.locator('h1')).toHaveText('Todo Items');
    await expect(page.locator('[data-test-description]')).toContainText([
      'owned-by-john',
    ]);
    await expect(page.locator('tbody')).not.toContainText('owned-by-guest');
  });

  test('Input validation: Attempting to save a todo-item with fewer than 4 characters displays a validation error', async ({
    page,
  }) => {
    await page.goto('/login');
    await page.locator('[data-test-username]').fill('guest');
    await page.locator('[data-test-password]').fill('Guest1!');
    await page.locator('[data-test-submit]').click();
    await expect(page).toHaveURL(/\/s\/todo-items/);

    await page.locator('[data-test-add]').click();
    await expect(page).toHaveURL(/\/s\/todo-items\/add/);

    // Empty description validation
    await page.locator('[data-test-submit]').click();
    let alertMessage = page.locator('.ember-notify .message');
    await expect(alertMessage).toBeVisible();
    await expect(alertMessage).toContainText("Description can't be blank");

    // Short description (< 4 characters) validation
    await page.locator('[data-test-description-input]').fill('abc');
    await page.locator('[data-test-submit]').click();
    alertMessage = page.locator('.ember-notify .message').last();
    await expect(alertMessage).toBeVisible();
    await expect(alertMessage).toContainText(
      'Description is too short (minimum is 4 characters)',
    );
    await expect(page).toHaveURL(/\/s\/todo-items\/add/);
  });

  test('Adding a valid todo-item saves successfully and navigates back to the updated list', async ({
    page,
  }) => {
    await page.goto('/login');
    await page.locator('[data-test-username]').fill('guest');
    await page.locator('[data-test-password]').fill('Guest1!');
    await page.locator('[data-test-submit]').click();
    await expect(page).toHaveURL(/\/s\/todo-items/);

    await page.locator('[data-test-add]').click();
    await expect(page).toHaveURL(/\/s\/todo-items\/add/);

    const newDescription = `valid-todo-item-${Date.now()}`;
    await page.locator('[data-test-description-input]').fill(newDescription);
    await page.locator('[data-test-submit]').click();

    await expect(page).toHaveURL(/\/s\/todo-items/);
    await expect(page.locator('[data-test-description]')).toContainText([
      newDescription,
    ]);
  });

  test('Clicking Logout invalidates the session and returns to the login screen', async ({
    page,
  }) => {
    await page.goto('/login');
    await page.locator('[data-test-username]').fill('guest');
    await page.locator('[data-test-password]').fill('Guest1!');
    await page.locator('[data-test-submit]').click();
    await expect(page).toHaveURL(/\/s\/todo-items/);

    await page.locator('[data-test-logout]').click();
    await expect(page).toHaveURL(/\/login/);
    await expect(page.locator('h1')).toHaveText('Please sign in');
  });

  test('Navigating directly to a protected route while logged out redirects to the login screen', async ({
    page,
  }) => {
    await page.goto('/s/todo-items');
    await expect(page).toHaveURL(/\/login/);
    await expect(page.locator('h1')).toHaveText('Please sign in');

    await page.goto('/s/todo-items/add');
    await expect(page).toHaveURL(/\/login/);
    await expect(page.locator('h1')).toHaveText('Please sign in');
  });
});
