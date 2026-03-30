import { useQuery } from '@tanstack/react-query';
import { useAuthStore } from '@/stores/authStore';
import api from '@/services/api';

interface UserProfile {
  id: string;
  email: string;
  fullName: string;
  bio?: string;
  timeBalance: number;
  rating: number;
  skills: { id: number; title: string; isOffering: boolean; categoryName: string }[];
}

export default function ProfilePage() {
  const userId = useAuthStore((s) => s.userId);

  const { data: profile, isLoading } = useQuery({
    queryKey: ['profile', userId],
    queryFn: () => api.get<UserProfile>(`/users/${userId}`).then((r) => r.data),
    enabled: !!userId,
  });

  if (isLoading) return <p className="text-gray-500">Loading profile...</p>;
  if (!profile) return <p className="text-gray-500">Profile not found.</p>;

  return (
    <div>
      <div className="bg-white border border-gray-200 rounded-xl p-6 mb-6">
        <h1 className="text-2xl font-bold text-gray-900">{profile.fullName}</h1>
        <p className="text-gray-500">{profile.email}</p>
        {profile.bio && <p className="mt-2 text-gray-600">{profile.bio}</p>}
        <div className="flex gap-6 mt-4">
          <div>
            <span className="text-2xl font-bold text-primary-600">{profile.timeBalance}</span>
            <span className="text-sm text-gray-500 ml-1">Time Credits</span>
          </div>
          <div>
            <span className="text-2xl font-bold text-yellow-500">{profile.rating.toFixed(1)}</span>
            <span className="text-sm text-gray-500 ml-1">Rating</span>
          </div>
        </div>
      </div>

      <h2 className="text-lg font-semibold text-gray-900 mb-4">Your Skills</h2>
      {profile.skills.length > 0 ? (
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          {profile.skills.map((s) => (
            <div key={s.id} className="bg-white border border-gray-200 rounded-lg p-4 flex justify-between items-center">
              <div>
                <p className="font-medium text-gray-900">{s.title}</p>
                <p className="text-sm text-gray-500">{s.categoryName}</p>
              </div>
              <span className={`text-xs font-medium px-2.5 py-1 rounded-full ${s.isOffering ? 'bg-green-100 text-green-700' : 'bg-blue-100 text-blue-700'
                }`}>
                {s.isOffering ? 'Offering' : 'Seeking'}
              </span>
            </div>
          ))}
        </div>
      ) : (
        <p className="text-gray-500">No skills added yet.</p>
      )}
    </div>
  );
}
