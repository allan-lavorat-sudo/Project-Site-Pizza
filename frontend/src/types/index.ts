// Types for API responses
export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  imageUrl: string;
  rating: number;
  categoryId: number;
  categoryName: string;
  isActive: boolean;
}

export interface Category {
  id: number;
  name: string;
  description: string;
  iconUrl: string;
  order: number;
}

export interface CartItem {
  product: Product;
  quantity: number;
  notes?: string;
}

export interface Order {
  id: number;
  orderNumber: string;
  totalAmount: number;
  deliveryFee: number;
  discountAmount?: number;
  status: OrderStatus;
  deliveryAddress: string;
  createdAt: string;
  items: OrderItem[];
}

export interface OrderItem {
  id: number;
  product: Product;
  quantity: number;
  unitPrice: number;
}

export interface User {
  id: number;
  fullName: string;
  email: string;
  phoneNumber: string;
  role: string;
}

export interface Promotion {
  id: number;
  title: string;
  description: string;
  imageUrl?: string;
  discountPercentage?: number;
  discountAmount?: number;
  minimumOrderValue?: number;
  startDate: string;
  endDate: string;
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  user: User;
}

export enum OrderStatus {
  Pending = 'Pending',
  Confirmed = 'Confirmed',
  Preparing = 'Preparing',
  Ready = 'Ready',
  OutForDelivery = 'OutForDelivery',
  Delivered = 'Delivered',
  Cancelled = 'Cancelled',
}

export interface ApiResponse<T> {
  success: boolean;
  data?: T;
  message?: string;
  errors?: string[];
}

export interface PaginatedResponse<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  fullName: string;
  email: string;
  phoneNumber: string;
  password: string;
  confirmPassword: string;
}

export interface CreateOrderRequest {
  items: CreateOrderItem[];
  deliveryAddress: string;
  deliveryNotes?: string;
  promotionId?: number;
}

export interface CreateOrderItem {
  productId: number;
  quantity: number;
  notes?: string;
}
