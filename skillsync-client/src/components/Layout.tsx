import { Outlet, Link, useLocation } from 'react-router-dom';
import { useAuthStore } from '@/stores/authStore';
import { useUnreadCount } from '@/hooks/useMessages';
import { Home, Compass, Repeat, User, LogOut, MessageSquare } from 'lucide-react';

const navItems = [
  { to: '/', label: 'Dashboard', icon: Home },
  { to: '/explore', label: 'Explore', icon: Compass },
  { to: '/swaps', label: 'Swaps', icon: Repeat },
  { to: '/messages', label: 'Messages', icon: MessageSquare, badge: true },
  { to: '/profile', label: 'Profile', icon: User },
];

export default function Layout() {
  const { fullName, logout } = useAuthStore();
  const location = useLocation();
  const { data: unreadCount } = useUnreadCount();

  return (
    <div className="min-h-screen flex">
      {/* Sidebar */}
      <aside className="w-64 bg-white border-r border-gray-200 p-6 flex flex-col">
        <h1 className="text-2xl font-bold text-primary-600 mb-8">SkillSync</h1>
        <nav className="flex-1 space-y-1">
          {navItems.map(({ to, label, icon: Icon, badge }) => {
            const active = location.pathname === to;
            return (
              <Link
                key={to}
                to={to}
                className={`flex items-center gap-3 px-3 py-2 rounded-lg text-sm font-medium transition-colors ${active
                  ? 'bg-primary-50 text-primary-700'
                  : 'text-gray-600 hover:bg-gray-100'
                  }`}
              >
                <Icon size={18} />
                {label}
                {badge && unreadCount && unreadCount > 0 ? (
                  <span className="ml-auto inline-flex items-center justify-center w-5 h-5 text-xs font-bold text-white bg-red-500 rounded-full">
                    {unreadCount > 9 ? '9+' : unreadCount}
                  </span>
                ) : null}
              </Link>
            );
          })}
        </nav>
        <div className="pt-4 border-t border-gray-200">
          <p className="text-sm text-gray-500 mb-2">{fullName}</p>
          <button
            onClick={logout}
            className="flex items-center gap-2 text-sm text-red-600 hover:text-red-800"
          >
            <LogOut size={16} /> Sign out
          </button>
        </div>
      </aside>

      {/* Main content */}
      <main className="flex-1 p-8 overflow-auto">
        <Outlet />
      </main>
    </div>
  );
}
