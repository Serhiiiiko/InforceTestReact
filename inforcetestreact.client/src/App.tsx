import React, { useEffect, useState } from 'react';
import './App.css';
import { apiService, LoginRequest, UrlMapping } from './services/api';

function App() {
    const [urls, setUrls] = useState<UrlMapping[]>([]);
    const [originalUrl, setOriginalUrl] = useState('');
    const [token, setToken] = useState<string | null>(localStorage.getItem('token'));
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');

    useEffect(() => {
        if (token) {
            fetchAllUrls();
        }
    }, [token]);

    const handleLogin = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const credentials: LoginRequest = { username, password };
            const response = await apiService.login(credentials);
            localStorage.setItem('token', response.token);
            setToken(response.token);
            setUsername('');
            setPassword('');
        } catch (error) {
            console.error('Login failed:', error);
            alert('Invalid username or password');
        }
    };

    const handleLogout = () => {
        localStorage.removeItem('token');
        setToken(null);
        setUrls([]);
    };

    const fetchAllUrls = async () => {
        try {
            const allUrls = await apiService.getAllUrls();
            setUrls(allUrls);
        } catch (error) {
            console.error('Failed to fetch URLs:', error);
        }
    };

    const handleShortenUrl = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!originalUrl) return;

        try {
            await apiService.createShortUrl({ originalUrl });
            setOriginalUrl('');
            fetchAllUrls(); // Refresh the list after adding a new URL
        } catch (error) {
            console.error('Failed to shorten URL:', error);
            alert('Error shortening URL. It might already exist.');
        }
    };

    const handleDeleteUrl = async (id: number) => {
        try {
            await apiService.deleteUrl(id);
            fetchAllUrls(); // Refresh the list
        } catch (error) {
            console.error('Failed to delete URL:', error);
        }
    };

    if (!token) {
        return (
            <div className="container">
                <h1>Login</h1>
                <form onSubmit={handleLogin}>
                    <input
                        type="text"
                        placeholder="Username"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                    />
                    <input
                        type="password"
                        placeholder="Password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                    />
                    <button type="submit">Login</button>
                </form>
                <p>Hint: Use credentials admin/admin123 or user/user123</p>
            </div>
        );
    }

    return (
        <div className="container">
            <button onClick={handleLogout} className="logout-button">Logout</button>
            <h1>URL Shortener</h1>
            <form onSubmit={handleShortenUrl}>
                <input
                    type="text"
                    placeholder="Enter URL to shorten"
                    value={originalUrl}
                    onChange={(e) => setOriginalUrl(e.target.value)}
                />
                <button type="submit">Shorten</button>
            </form>

            <h2>Shortened URLs</h2>
            <table className="table table-striped">
                <thead>
                    <tr>
                        <th>Original URL</th>
                        <th>Short URL</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {urls.map(url => (
                        <tr key={url.id}>
                            <td><a href={url.originalUrl} target="_blank" rel="noopener noreferrer">{url.originalUrl}</a></td>
                            <td><a href={url.shortUrl} target="_blank" rel="noopener noreferrer">{url.shortUrl}</a></td>
                            <td>
                                <button onClick={() => handleDeleteUrl(url.id)}>Delete</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}

export default App;