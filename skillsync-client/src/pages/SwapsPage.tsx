import { useSwaps, useUpdateSwapStatus } from '@/hooks/useSwaps';
import { useAuthStore } from '@/stores/authStore';

export default function SwapsPage() {
  const userId = useAuthStore((s) => s.userId);
  const { data: swaps, isLoading } = useSwaps();
  const updateStatus = useUpdateSwapStatus();

  if (isLoading) return <p className="text-gray-500">Loading swaps...</p>;

  const pending = swaps?.filter((s) => s.status === 'Pending') ?? [];
  const active = swaps?.filter((s) => s.status === 'Accepted') ?? [];
  const completed = swaps?.filter((s) => s.status === 'Completed' || s.status === 'Rejected') ?? [];

  return (
    <div>
      <h1 className="text-2xl font-bold text-gray-900 mb-6">Your Swaps</h1>

      {pending.length > 0 && (
        <Section title="Pending Requests">
          {pending.map((swap) => (
            <SwapRow key={swap.id} swap={swap} userId={userId!}>
              {swap.receiverId === userId && (
                <div className="flex gap-2">
                  <button
                    onClick={() => updateStatus.mutate({ id: swap.id, dto: { status: 1 } })}
                    className="text-sm px-3 py-1 bg-green-600 text-white rounded-lg hover:bg-green-700"
                  >
                    Accept
                  </button>
                  <button
                    onClick={() => updateStatus.mutate({ id: swap.id, dto: { status: 3 } })}
                    className="text-sm px-3 py-1 bg-red-600 text-white rounded-lg hover:bg-red-700"
                  >
                    Reject
                  </button>
                </div>
              )}
            </SwapRow>
          ))}
        </Section>
      )}

      {active.length > 0 && (
        <Section title="Active Swaps">
          {active.map((swap) => (
            <SwapRow key={swap.id} swap={swap} userId={userId!}>
              <button
                onClick={() => updateStatus.mutate({ id: swap.id, dto: { status: 2 } })}
                className="text-sm px-3 py-1 bg-primary-600 text-white rounded-lg hover:bg-primary-700"
              >
                Mark Complete
              </button>
            </SwapRow>
          ))}
        </Section>
      )}

      {completed.length > 0 && (
        <Section title="History">
          {completed.map((swap) => (
            <SwapRow key={swap.id} swap={swap} userId={userId!} />
          ))}
        </Section>
      )}

      {(!swaps || swaps.length === 0) && (
        <p className="text-gray-500">No swaps yet. Explore skills to get started!</p>
      )}
    </div>
  );
}

function Section({ title, children }: { title: string; children: React.ReactNode }) {
  return (
    <div className="mb-8">
      <h2 className="text-lg font-semibold text-gray-900 mb-3">{title}</h2>
      <div className="space-y-3">{children}</div>
    </div>
  );
}

function SwapRow({ swap, userId, children }: { swap: any; userId: string; children?: React.ReactNode }) {
  const otherName = swap.requesterId === userId ? swap.receiverName : swap.requesterName;
  return (
    <div className="bg-white border border-gray-200 rounded-lg p-4 flex justify-between items-center">
      <div>
        <p className="font-medium text-gray-900">{swap.offeredSkillTitle} ↔ {swap.requestedSkillTitle}</p>
        <p className="text-sm text-gray-500">with {otherName}</p>
      </div>
      <div className="flex items-center gap-3">
        <span className={`text-xs font-medium px-2.5 py-1 rounded-full ${swap.status === 'Pending' ? 'bg-yellow-100 text-yellow-700' :
            swap.status === 'Accepted' ? 'bg-green-100 text-green-700' :
              swap.status === 'Completed' ? 'bg-blue-100 text-blue-700' :
                'bg-red-100 text-red-700'
          }`}>
          {swap.status}
        </span>
        {children}
      </div>
    </div>
  );
}
