import apiClient from './api';
import {
  Product,
  Category,
  ApiResponse,
  PaginatedResponse,
  Promotion,
  AuthResponse,
  LoginRequest,
  RegisterRequest,
  Order,
  CreateOrderRequest,
} from '@/types';

// Product Service
export const productService = {
  getAll: async (): Promise<Product[]> => {
    const response = await apiClient.get<ApiResponse<Product[]>>('/products');
    return response.data.data || [];
  },

  getById: async (id: number): Promise<Product> => {
    const response = await apiClient.get<ApiResponse<Product>>(`/products/${id}`);
    return response.data.data!;
  },

  getByCategory: async (categoryId: number): Promise<Product[]> => {
    const response = await apiClient.get<ApiResponse<Product[]>>(`/products/category/${categoryId}`);
    return response.data.data || [];
  },

  getPaginated: async (pageNumber: number, pageSize: number): Promise<PaginatedResponse<Product>> => {
    const response = await apiClient.get<ApiResponse<PaginatedResponse<Product>>>(`/products/paginated`, {
      params: { pageNumber, pageSize },
    });
    return response.data.data!;
  },
};

// Category Service
export const categoryService = {
  getAll: async (): Promise<Category[]> => {
    const response = await apiClient.get<ApiResponse<Category[]>>('/categories');
    return response.data.data || [];
  },

  getById: async (id: number): Promise<Category> => {
    const response = await apiClient.get<ApiResponse<Category>>(`/categories/${id}`);
    return response.data.data!;
  },
};

// Auth Service
export const authService = {
  login: async (data: LoginRequest): Promise<AuthResponse> => {
    const response = await apiClient.post<ApiResponse<AuthResponse>>('/auth/login', data);
    const result = response.data.data!;

    // Store tokens and user
    if (typeof window !== 'undefined') {
      localStorage.setItem('accessToken', result.accessToken);
      localStorage.setItem('refreshToken', result.refreshToken);
      localStorage.setItem('user', JSON.stringify(result.user));
    }

    return result;
  },

  register: async (data: RegisterRequest): Promise<AuthResponse> => {
    const response = await apiClient.post<ApiResponse<AuthResponse>>('/auth/register', data);
    const result = response.data.data!;

    // Store tokens and user
    if (typeof window !== 'undefined') {
      localStorage.setItem('accessToken', result.accessToken);
      localStorage.setItem('refreshToken', result.refreshToken);
      localStorage.setItem('user', JSON.stringify(result.user));
    }

    return result;
  },

  logout: async (): Promise<void> => {
    if (typeof window !== 'undefined') {
      localStorage.removeItem('accessToken');
      localStorage.removeItem('refreshToken');
      localStorage.removeItem('user');
    }
  },

  getCurrentUser: () => {
    if (typeof window !== 'undefined') {
      const user = localStorage.getItem('user');
      return user ? JSON.parse(user) : null;
    }
    return null;
  },

  getAccessToken: (): string | null => {
    if (typeof window !== 'undefined') {
      return localStorage.getItem('accessToken');
    }
    return null;
  },
};

// Order Service
export const orderService = {
  create: async (data: CreateOrderRequest): Promise<Order> => {
    const response = await apiClient.post<ApiResponse<Order>>('/orders', data);
    return response.data.data!;
  },

  getById: async (id: number): Promise<Order> => {
    const response = await apiClient.get<ApiResponse<Order>>(`/orders/${id}`);
    return response.data.data!;
  },

  getUserOrders: async (): Promise<Order[]> => {
    const response = await apiClient.get<ApiResponse<Order[]>>('/orders');
    return response.data.data || [];
  },

  updateStatus: async (id: number, status: string): Promise<Order> => {
    const response = await apiClient.put<ApiResponse<Order>>(`/orders/${id}/status`, { status });
    return response.data.data!;
  },

  getPaginated: async (pageNumber: number, pageSize: number): Promise<PaginatedResponse<Order>> => {
    const response = await apiClient.get<ApiResponse<PaginatedResponse<Order>>>('/orders/paginated', {
      params: { pageNumber, pageSize },
    });
    return response.data.data!;
  },
};

// Promotion Service
export const promotionService = {
  getActive: async (): Promise<Promotion[]> => {
    const response = await apiClient.get<ApiResponse<Promotion[]>>('/promotions/active');
    return response.data.data || [];
  },

  getAll: async (): Promise<Promotion[]> => {
    const response = await apiClient.get<ApiResponse<Promotion[]>>('/promotions');
    return response.data.data || [];
  },

  getById: async (id: number): Promise<Promotion> => {
    const response = await apiClient.get<ApiResponse<Promotion>>(`/promotions/${id}`);
    return response.data.data!;
  },
};

// Ifood Service
export const ifoodService = {
  getAuthorizationUrl: async (): Promise<string> => {
    const response = await apiClient.get<ApiResponse<string>>('/ifood/auth-url');
    return response.data.data || '';
  },

  authenticate: async (authCode: string): Promise<boolean> => {
    const response = await apiClient.post<ApiResponse<boolean>>('/ifood/authenticate', { authCode });
    return response.data.data || false;
  },
};
