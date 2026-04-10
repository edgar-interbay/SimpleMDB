import { $, apiFetch, renderStatus, clearChildren } from '/scripts/common.js';

(async function initUsersIndex() {
  const listEl   = $('#user-list');
  const statusEl = $('#status');
  const tpl      = $('#user-card');

  try {
    const users = await apiFetch('/users');
    clearChildren(listEl);

    if (users.length === 0) {
      renderStatus(statusEl, 'warn', 'No users found.');
    } else {
      renderStatus(statusEl, '', '');
      for (const u of users) {
        const frag = tpl.content.cloneNode(true);
        const root = frag.querySelector('.card');
        root.querySelector('.username').textContent  = u.username;
        root.querySelector('.role').textContent      = u.role;
        root.querySelector('.btn-view').href         = `/users/view.html?id=${u.id}`;
        root.querySelector('.btn-edit').href         = `/users/edit.html?id=${u.id}`;
        root.querySelector('.btn-delete').dataset.id = u.id;
        listEl.appendChild(frag);
      }
    }

    listEl.addEventListener('click', async (ev) => {
      const btn = ev.target.closest('button.btn-delete[data-id]');
      if (!btn) return;
      const id = btn.dataset.id;
      if (!confirm('Delete this user? This cannot be undone.')) return;
      try {
        await apiFetch(`/users/${id}`, { method: 'DELETE' });
        renderStatus(statusEl, 'ok', `User ${id} deleted.`);
        setTimeout(() => location.reload(), 1500);
      } catch (err) {
        renderStatus(statusEl, 'err', `Delete failed: ${err.message}`);
      }
    });
  } catch (err) {
    renderStatus(statusEl, 'err', `Failed to fetch users: ${err.message}`);
  }
})();
