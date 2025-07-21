export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  username: string;
  role: string;
  expiresAt: string;
}

export interface CreateUrlRequest {
  originalUrl: string;
}

export interface UrlMapping {
  id: number;
  originalUrl: string;
  shortCode: string;
  shortUrl: string;
  createdBy: string;
  createdDate: string;
  clickCount: number;
  lastAccessedDate?: string;
}

export interface UrlDetails extends UrlMapping {
  createdByFullName: string;
}

class ApiService {
  private baseUrl = '';
  
  private getAuthHeaders(): HeadersInit {
    const token = localStorage.getItem('token');
    return {
      'Content-Type': 'application/json',
      ...(token && { Authorization: `Bearer ${token}` })
    };
  }

  async login(credentials: LoginRequest): Promise<LoginResponse> {
    const response = await fetch(`${this.baseUrl}/api/auth/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(credentials)
    });

    if (!response.ok) {
      const error = await response.json();
      throw new Error(error.message || 'Login failed');
    }

    return response.json();
  }

  async getAllUrls(): Promise<UrlMapping[]> {
    const response = await fetch(`${this.baseUrl}/api/urls`, {
      headers: this.getAuthHeaders()
    });

    if (!response.ok) {
      throw new Error('Failed to fetch URLs');
    }

    return response.json();
  }

  async getUrlDetails(id: number): Promise<UrlDetails> {
    const response = await fetch(`${this.baseUrl}/api/urls/${id}`, {
      headers: this.getAuthHeaders()
    });

    if (!response.ok) {
      if (response.status === 401) {
        throw new Error('Unauthorized - Please login');
      }
      throw new Error('Failed to fetch URL details');
    }

    return response.json();
  }

  async createShortUrl(request: CreateUrlRequest): Promise<UrlMapping> {
    const response = await fetch(`${this.baseUrl}/api/urls`, {
      method: 'POST',
      headers: this.getAuthHeaders(),
      body: JSON.stringify(request)
    });

    if (!response.ok) {
      const error = await response.json();
      throw new Error(error.message || 'Failed to create short URL');
    }

    return response.json();
  }

  async deleteUrl(id: number): Promise<void> {
    const response = await fetch(`${this.baseUrl}/api/urls/${id}`, {
      method: 'DELETE',
      headers: this.getAuthHeaders()
    });

    if (!response.ok) {
      throw new Error('Failed to delete URL');
    }
  }
}

export const apiService = new ApiService();