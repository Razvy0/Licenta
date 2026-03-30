import { create } from 'zustand';
import { persist } from 'zustand/middleware';

interface AuthState {
  token: string | null;
  userId: string | null;
  email: string | null;
  fullName: string | null;
  isAuthenticated: boolean;
  setAuth: (data: { token: string; userId: string; email: string; fullName: string }) => void;
  logout: () => void;
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set) => ({
      token: null,
      userId: null,
      email: null,
      fullName: null,
      isAuthenticated: false,
      setAuth: (data) =>
        set({
          token: data.token,
          userId: data.userId,
          email: data.email,
          fullName: data.fullName,
          isAuthenticated: true,
        }),
      logout: () =>
        set({
          token: null,
          userId: null,
          email: null,
          fullName: null,
          isAuthenticated: false,
        }),
    }),
    { name: 'skillsync-auth' }
  )
);
