import { create } from 'zustand';
import { persist } from 'zustand/middleware';

interface AuthState {
  token: string | null;
  userId: string | null;
  email: string | null;
  fullName: string | null;
  isAuthenticated: boolean;
  lastSeenSwapsAt: string | null;
  setAuth: (data: { token: string; userId: string; email: string; fullName: string }) => void;
  setLastSeenSwapsAt: (value: string) => void;
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
      lastSeenSwapsAt: null,
      setAuth: (data) =>
        set({
          token: data.token,
          userId: data.userId,
          email: data.email,
          fullName: data.fullName,
          isAuthenticated: true,
        }),
      setLastSeenSwapsAt: (value) => set({ lastSeenSwapsAt: value }),
      logout: () =>
        set({
          token: null,
          userId: null,
          email: null,
          fullName: null,
          isAuthenticated: false,
          lastSeenSwapsAt: null,
        }),
    }),
    { name: 'skillsync-auth' }
  )
);
