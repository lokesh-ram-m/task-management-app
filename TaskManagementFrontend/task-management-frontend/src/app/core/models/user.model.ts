export enum UserRole {
  User  = 0,
  Admin = 1
}

export interface UserResponse {
  id:       number;
  username: string;
  email:    string;
  role:     UserRole;
  isActive: boolean;
}

export interface LoginRequest {
  login:    string;
  password: string;
}

export interface LoginResponse {
  token:     string;
  username:  string;
  role:      UserRole;
  expiresAt: string;
}

export interface RegisterRequest {
  username: string;
  email:    string;
  password: string;
}
