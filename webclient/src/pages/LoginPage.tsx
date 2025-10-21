import { useState } from 'react';

async function login(username: string, password: string) {
  try {
    const res = await fetch('/api/auth/login', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ username, password })
    });
    if (!res.ok) return { ok: false };
    return { ok: true };
  } catch (e) {
    return { ok: false };
  }
}

export function LoginPage() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState<string | null>(null);

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    const res = await login(username, password);
    if (!res.ok) {
      setError('Invalid username or password');
    } else {
      window.location.href = '/swagger';
    }
  };

  return (
    <div className="page-root">
      <form className="card" onSubmit={onSubmit}>
        <h2>Sign in to CNC 3D</h2>
        <label className="label">Username</label>
        <input className="input" id="username" value={username} onChange={e => setUsername(e.target.value)} />
        <label className="label">Password</label>
        <input className="input" id="password" type="password" value={password} onChange={e => setPassword(e.target.value)} />
        {error && <div className="error">{error}</div>}
        <button className="btn" type="submit">Sign in</button>
      </form>
    </div>
  );
}
