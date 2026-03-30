import { useAuthStore } from '@/stores/authStore';
import { useSwaps } from '@/hooks/useSwaps';
import { useSkills } from '@/hooks/useSkills';
import { Clock, ArrowRightLeft, Star } from 'lucide-react';

export default function DashboardPage() {
  const fullName = useAuthStore((s) => s.fullName);
  const { data: swaps } = useSwaps();
  const { data: skills } = useSkills();

  const activeSwaps = swaps?.filter((s) => s.status === 'Pending' || s.status === 'Accepted') ?? [];

  return (
    <div>
      <h1 className="text-2xl font-bold text-gray-900 mb-1">Welcome back, {fullName}!</h1>
      <p className="text-gray-500 mb-8">Here's an overview of your activity.</p>

      <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
        <StatCard icon={<Star className="text-yellow-500" />} label="Skills Listed" value={skills?.length ?? 0} />
        <StatCard icon={<ArrowRightLeft className="text-primary-500" />} label="Active Swaps" value={activeSwaps.length} />
        <StatCard icon={<Clock className="text-green-500" />} label="Completed" value={swaps?.filter((s) => s.status === 'Completed').length ?? 0} />
      </div>

      {activeSwaps.length > 0 && (
        <div>
          <h2 className="text-lg font-semibold text-gray-900 mb-4">Active Swaps</h2>
          <div className="space-y-3">
            {activeSwaps.map((swap) => (
              <div key={swap.id} className="bg-white border border-gray-200 rounded-lg p-4 flex justify-between items-center">
                <div>
                  <p className="font-medium text-gray-900">
                    {swap.offeredSkillTitle} ↔ {swap.requestedSkillTitle}
                  </p>
                  <p className="text-sm text-gray-500">
                    with {swap.requesterName === fullName ? swap.receiverName : swap.requesterName}
                  </p>
                </div>
                <span className={`text-xs font-medium px-2.5 py-1 rounded-full ${swap.status === 'Pending' ? 'bg-yellow-100 text-yellow-700' : 'bg-green-100 text-green-700'
                  }`}>
                  {swap.status}
                </span>
              </div>
            ))}
          </div>
        </div>
      )}
    </div>
  );
}

function StatCard({ icon, label, value }: { icon: React.ReactNode; label: string; value: number }) {
  return (
    <div className="bg-white border border-gray-200 rounded-xl p-5 flex items-center gap-4">
      <div className="p-3 bg-gray-50 rounded-lg">{icon}</div>
      <div>
        <p className="text-2xl font-bold text-gray-900">{value}</p>
        <p className="text-sm text-gray-500">{label}</p>
      </div>
    </div>
  );
}
