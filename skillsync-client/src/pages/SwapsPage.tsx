import { useState } from 'react';
import { useSwaps, useUpdateSwapStatus, useProposeTimeSlot, usePickTime, useValidateSwap, useInvalidateSwap } from '@/hooks/useSwaps';
import { useAuthStore } from '@/stores/authStore';
import { Swap } from '@/services/swapService';
import { Calendar, Check, X, Clock, ArrowRightLeft, Star, AlertTriangle } from 'lucide-react';
import { Link } from 'react-router-dom';
import ReviewModal from '@/components/ReviewModal';
import ReportModal from '@/components/ReportModal';
import { useHasReviewed } from '@/hooks/useReviews';

export default function SwapsPage() {
  const userId = useAuthStore((s) => s.userId);
  const { data: swaps, isLoading } = useSwaps();

  if (isLoading) return <p className="text-gray-500">Loading swaps...</p>;

  const pending = swaps?.filter((s) => s.status === 'Pending') ?? [];
  const accepted = swaps?.filter((s) => s.status === 'Accepted') ?? [];
  const scheduled = swaps?.filter((s) =>
    s.status === 'Scheduled' || s.status === 'ValidatedByRequester' || s.status === 'ValidatedByReceiver'
  ) ?? [];
  const completed = swaps?.filter((s) => s.status === 'Completed') ?? [];
  const rejected = swaps?.filter((s) => s.status === 'Rejected' || s.status === 'Cancelled') ?? [];
  const disputed = swaps?.filter((s) => s.status === 'Disputed') ?? [];

  return (
    <div>
      <h1 className="text-2xl font-bold text-gray-900 mb-6">Your Swaps</h1>

      {pending.length > 0 && (
        <Section title="Pending Requests">
          {pending.map((swap) => (
            <PendingSwapCard key={swap.id} swap={swap} userId={userId!} />
          ))}
        </Section>
      )}

      {accepted.length > 0 && (
        <Section title="Accepted — Schedule Meeting">
          {accepted.map((swap) => (
            <AcceptedSwapCard key={swap.id} swap={swap} userId={userId!} />
          ))}
        </Section>
      )}

      {scheduled.length > 0 && (
        <Section title="Scheduled — Validate Meeting">
          {scheduled.map((swap) => (
            <ScheduledSwapCard key={swap.id} swap={swap} userId={userId!} />
          ))}
        </Section>
      )}

      {completed.length > 0 && (
        <Section title="Completed">
          {completed.map((swap) => (
            <CompletedSwapCard key={swap.id} swap={swap} userId={userId!} />
          ))}
        </Section>
      )}

      {rejected.length > 0 && (
        <Section title="Rejected / Cancelled">
          {rejected.map((swap) => (
            <SwapRow key={swap.id} swap={swap} userId={userId!}>
              <span className="text-xs font-medium px-2.5 py-1 rounded-full bg-red-100 text-red-700">{swap.status}</span>
            </SwapRow>
          ))}
        </Section>
      )}

      {disputed.length > 0 && (
        <Section title="Disputed (Under Review)">
          {disputed.map((swap) => (
            <SwapRow key={swap.id} swap={swap} userId={userId!}>
              <span className="flex items-center gap-1 text-xs font-medium px-2.5 py-1 rounded-full bg-orange-100 text-orange-800">
                <AlertTriangle size={12} /> Disputed
              </span>
            </SwapRow>
          ))}
        </Section>
      )}

      {(!swaps || swaps.length === 0) && (
        <p className="text-gray-500">No swaps yet. Explore skills to get started!</p>
      )}
    </div>
  );
}

/* ─── Pending: Accept / Reject ─── */
function PendingSwapCard({ swap, userId }: { swap: Swap; userId: string }) {
  const updateStatus = useUpdateSwapStatus();
  const isReceiver = swap.receiverId === userId;

  return (
    <SwapRow swap={swap} userId={userId}>
      <span className="text-xs font-medium px-2.5 py-1 rounded-full bg-yellow-100 text-yellow-700">Pending</span>
      {isReceiver ? (
        <div className="flex gap-2">
          <button
            onClick={() => updateStatus.mutate({ id: swap.id, dto: { status: 'Accepted' } })}
            className="flex items-center gap-1 text-sm px-3 py-1.5 bg-green-600 text-white rounded-lg hover:bg-green-700"
          >
            <Check size={14} /> Accept
          </button>
          <button
            onClick={() => updateStatus.mutate({ id: swap.id, dto: { status: 'Rejected' } })}
            className="flex items-center gap-1 text-sm px-3 py-1.5 bg-red-600 text-white rounded-lg hover:bg-red-700"
          >
            <X size={14} /> Reject
          </button>
        </div>
      ) : (
        <button
          onClick={() => updateStatus.mutate({ id: swap.id, dto: { status: 'Cancelled' } })}
          className="text-sm px-3 py-1.5 text-gray-600 border border-gray-300 rounded-lg hover:bg-gray-50"
        >
          Cancel
        </button>
      )}
    </SwapRow>
  );
}

/* ─── Accepted: Receiver proposes time-slot, Requester picks time ─── */
function AcceptedSwapCard({ swap, userId }: { swap: Swap; userId: string }) {
  const isReceiver = swap.receiverId === userId;
  const proposeTimeSlot = useProposeTimeSlot();
  const pickTime = usePickTime();
  const [slotStart, setSlotStart] = useState('');
  const [slotEnd, setSlotEnd] = useState('');
  const [pickedTime, setPickedTime] = useState('');
  const [showReport, setShowReport] = useState(false);

  const hasTimeSlot = swap.timeSlotStart && swap.timeSlotEnd;

  return (
    <div className="bg-white border border-gray-200 rounded-lg p-4 space-y-3">
      <div className="flex justify-between items-start">
        <SwapHeader swap={swap} userId={userId} />
        <button
          onClick={() => setShowReport(true)}
          className="p-1.5 text-gray-400 hover:text-red-600 hover:bg-red-50 rounded-lg transition-colors cursor-pointer"
          title="Report Issue"
        >
          <AlertTriangle size={16} />
        </button>
      </div>
      <span className="inline-block text-xs font-medium px-2.5 py-1 rounded-full bg-green-100 text-green-700">Accepted</span>

      {isReceiver && !hasTimeSlot && (
        <div className="bg-gray-50 rounded-lg p-3 space-y-2">
          <p className="text-sm font-medium text-gray-700 flex items-center gap-1"><Clock size={14} /> Propose a time window for the meeting</p>
          <div className="flex gap-2 items-end flex-wrap">
            <label className="text-xs text-gray-500">
              From
              <input type="datetime-local" value={slotStart} onChange={(e) => setSlotStart(e.target.value)}
                className="block mt-1 px-3 py-1.5 border border-gray-300 rounded-lg text-sm focus:ring-2 focus:ring-primary-500 outline-none" />
            </label>
            <label className="text-xs text-gray-500">
              To
              <input type="datetime-local" value={slotEnd} onChange={(e) => setSlotEnd(e.target.value)}
                className="block mt-1 px-3 py-1.5 border border-gray-300 rounded-lg text-sm focus:ring-2 focus:ring-primary-500 outline-none" />
            </label>
            <button
              disabled={!slotStart || !slotEnd || proposeTimeSlot.isPending}
              onClick={() => proposeTimeSlot.mutate({
                id: swap.id,
                dto: { timeSlotStart: new Date(slotStart).toISOString(), timeSlotEnd: new Date(slotEnd).toISOString() }
              })}
              className="px-4 py-1.5 text-sm bg-primary-600 text-white rounded-lg hover:bg-primary-700 disabled:opacity-50"
            >
              Propose
            </button>
          </div>
        </div>
      )}

      {isReceiver && hasTimeSlot && (
        <p className="text-sm text-gray-500 flex items-center gap-1">
          <Calendar size={14} /> Time slot proposed: {formatDateTime(swap.timeSlotStart!)} – {formatDateTime(swap.timeSlotEnd!)}. Waiting for the other party to pick a time.
        </p>
      )}

      {!isReceiver && !hasTimeSlot && (
        <p className="text-sm text-gray-500">Waiting for the other party to propose a time window...</p>
      )}

      {!isReceiver && hasTimeSlot && (
        <div className="bg-gray-50 rounded-lg p-3 space-y-2">
          <p className="text-sm font-medium text-gray-700 flex items-center gap-1">
            <Calendar size={14} /> Pick a meeting time between {formatDateTime(swap.timeSlotStart!)} and {formatDateTime(swap.timeSlotEnd!)}
          </p>
          <div className="flex gap-2 items-end">
            <input type="datetime-local" value={pickedTime} onChange={(e) => setPickedTime(e.target.value)}
              min={swap.timeSlotStart?.slice(0, 16)} max={swap.timeSlotEnd?.slice(0, 16)}
              className="px-3 py-1.5 border border-gray-300 rounded-lg text-sm focus:ring-2 focus:ring-primary-500 outline-none" />
            <button
              disabled={!pickedTime || pickTime.isPending}
              onClick={() => pickTime.mutate({
                id: swap.id,
                dto: { scheduledDate: new Date(pickedTime).toISOString() }
              })}
              className="px-4 py-1.5 text-sm bg-primary-600 text-white rounded-lg hover:bg-primary-700 disabled:opacity-50"
            >
              Confirm Time
            </button>
          </div>
        </div>
      )}
      <ReportModal isOpen={showReport} onClose={() => setShowReport(false)} swapRequestId={swap.id} />
    </div>
  );
}

/* ─── Scheduled: Validate / Invalidate after meeting ─── */
function ScheduledSwapCard({ swap, userId }: { swap: Swap; userId: string }) {
  const validateSwap = useValidateSwap();
  const invalidateSwap = useInvalidateSwap();
  const isRequester = swap.requesterId === userId;
  const hasValidated = isRequester ? swap.requesterValidated : swap.receiverValidated;
  const otherValidated = isRequester ? swap.receiverValidated : swap.requesterValidated;
  const [showReport, setShowReport] = useState(false);

  return (
    <div className="bg-white border border-gray-200 rounded-lg p-4 space-y-3">
      <div className="flex justify-between items-start">
        <SwapHeader swap={swap} userId={userId} />
        <button
          onClick={() => setShowReport(true)}
          className="p-1.5 text-gray-400 hover:text-red-600 hover:bg-red-50 rounded-lg transition-colors cursor-pointer"
          title="Report Issue"
        >
          <AlertTriangle size={16} />
        </button>
      </div>
      <div className="flex flex-wrap items-center gap-2">
        <span className="text-xs font-medium px-2.5 py-1 rounded-full bg-purple-100 text-purple-700">
          Scheduled — {formatDateTime(swap.scheduledDate!)}
        </span>
      </div>

      <div className="bg-gray-50 rounded-lg p-3 space-y-2">
        <p className="text-sm text-gray-700 font-medium">Meeting validation</p>
        <div className="flex gap-3 text-sm">
          <span className={`flex items-center gap-1 ${swap.requesterValidated ? 'text-green-600' : 'text-gray-400'}`}>
            {swap.requesterValidated ? <Check size={14} /> : <Clock size={14} />}
            {swap.requesterName} {swap.requesterValidated ? 'validated' : 'pending'}
          </span>
          <span className={`flex items-center gap-1 ${swap.receiverValidated ? 'text-green-600' : 'text-gray-400'}`}>
            {swap.receiverValidated ? <Check size={14} /> : <Clock size={14} />}
            {swap.receiverName} {swap.receiverValidated ? 'validated' : 'pending'}
          </span>
        </div>

        {!hasValidated && (
          <div className="flex gap-2 pt-1">
            <button
              onClick={() => validateSwap.mutate(swap.id)}
              disabled={validateSwap.isPending}
              className="flex items-center gap-1 text-sm px-4 py-1.5 bg-green-600 text-white rounded-lg hover:bg-green-700 disabled:opacity-50"
            >
              <Check size={14} /> Validate — Contract Fulfilled
            </button>
            <button
              onClick={() => invalidateSwap.mutate(swap.id)}
              disabled={invalidateSwap.isPending}
              className="flex items-center gap-1 text-sm px-4 py-1.5 bg-red-600 text-white rounded-lg hover:bg-red-700 disabled:opacity-50"
            >
              <X size={14} /> Invalidate — Need to Redo
            </button>
          </div>
        )}

        {hasValidated && !otherValidated && (
          <p className="text-sm text-green-600 flex items-center gap-1 pt-1">
            <Check size={14} /> You validated. Waiting for the other party...
          </p>
        )}
      </div>
      <ReportModal isOpen={showReport} onClose={() => setShowReport(false)} swapRequestId={swap.id} />
    </div>
  );
}

/* ─── Completed: Leave Review ─── */
function CompletedSwapCard({ swap, userId }: { swap: Swap; userId: string }) {
  const [showReview, setShowReview] = useState(false);
  const [showReport, setShowReport] = useState(false);
  const { data: hasReviewed } = useHasReviewed(swap.id);
  const otherName = swap.requesterId === userId ? swap.receiverName : swap.requesterName;
  const otherUserId = swap.requesterId === userId ? swap.receiverId : swap.requesterId;

  return (
    <div className="bg-white border border-gray-200 rounded-lg p-4 flex justify-between items-center">
      <div>
        <p className="font-medium text-gray-900 flex items-center gap-1">
          {swap.offeredSkillTitle} <ArrowRightLeft size={14} className="text-gray-400" /> {swap.requestedSkillTitle}
        </p>
        <p className="text-sm text-gray-500">
          with{' '}
          <Link to={`/users/${otherUserId}`} className="text-primary-600 hover:text-primary-800 font-medium">
            {otherName}
          </Link>
        </p>
      </div>
      <div className="flex items-center gap-3">
        <span className="text-xs font-medium px-2.5 py-1 rounded-full bg-blue-100 text-blue-700">Completed</span>
        {hasReviewed ? (
          <span className="flex items-center gap-1 text-xs text-green-600 font-medium">
            <Star size={14} fill="currentColor" /> Reviewed
          </span>
        ) : (
          <button
            onClick={() => setShowReview(true)}
            className="flex items-center gap-1 text-sm px-3 py-1.5 bg-yellow-500 text-white rounded-lg hover:bg-yellow-600"
          >
            <Star size={14} /> Leave Review
          </button>
        )}
        <button
          onClick={() => setShowReport(true)}
          className="flex items-center gap-1 text-sm px-3 py-1.5 text-gray-700 border border-gray-300 bg-white hover:bg-gray-50 rounded-lg"
          title="Report Issue"
        >
          <AlertTriangle size={14} className="text-red-500" /> Report
        </button>
      </div>
      {showReview && (
        <ReviewModal
          swapRequestId={swap.id}
          otherUserName={otherName}
          onClose={() => setShowReview(false)}
        />
      )}
      <ReportModal isOpen={showReport} onClose={() => setShowReport(false)} swapRequestId={swap.id} />
    </div>
  );
}

/* ─── Shared components ─── */
function Section({ title, children }: { title: string; children: React.ReactNode }) {
  return (
    <div className="mb-8">
      <h2 className="text-lg font-semibold text-gray-900 mb-3">{title}</h2>
      <div className="space-y-3">{children}</div>
    </div>
  );
}

function SwapHeader({ swap, userId }: { swap: Swap; userId: string }) {
  const otherName = swap.requesterId === userId ? swap.receiverName : swap.requesterName;
  const otherUserId = swap.requesterId === userId ? swap.receiverId : swap.requesterId;
  return (
    <div>
      <p className="font-medium text-gray-900 flex items-center gap-1">
        {swap.offeredSkillTitle} <ArrowRightLeft size={14} className="text-gray-400" /> {swap.requestedSkillTitle}
      </p>
      <p className="text-sm text-gray-500">
        with{' '}
        <Link to={`/users/${otherUserId}`} className="text-primary-600 hover:text-primary-800 font-medium">
          {otherName}
        </Link>
      </p>
    </div>
  );
}

function SwapRow({ swap, userId, children }: { swap: Swap; userId: string; children?: React.ReactNode }) {
  const otherName = swap.requesterId === userId ? swap.receiverName : swap.requesterName;
  return (
    <div className="bg-white border border-gray-200 rounded-lg p-4 flex justify-between items-center">
      <div>
        <p className="font-medium text-gray-900 flex items-center gap-1">
          {swap.offeredSkillTitle} <ArrowRightLeft size={14} className="text-gray-400" /> {swap.requestedSkillTitle}
        </p>
        <p className="text-sm text-gray-500">with {otherName}</p>
      </div>
      <div className="flex items-center gap-3">
        {children}
      </div>
    </div>
  );
}

function formatDateTime(iso: string) {
  return new Date(iso).toLocaleString([], {
    month: 'short', day: 'numeric', hour: '2-digit', minute: '2-digit'
  });
}
