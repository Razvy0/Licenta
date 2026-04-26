import { useState } from 'react';
import { useMySkills } from '@/hooks/useSkills';
import { useCreateSwap } from '@/hooks/useSwaps';
import { Skill } from '@/services/skillService';
import { X } from 'lucide-react';

interface Props {
    targetSkill: Skill;
    onClose: () => void;
}

export default function ProposeSwapModal({ targetSkill, onClose }: Props) {
    const { data: mySkills } = useMySkills();
    const createSwap = useCreateSwap();
    const [selectedSkillId, setSelectedSkillId] = useState<number>(0);

    const offeringSkills = mySkills?.filter((s) => s.isOffering) ?? [];

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        createSwap.mutate(
            { offeredSkillId: selectedSkillId, requestedSkillId: targetSkill.id },
            { onSuccess: () => onClose() }
        );
    };

    return (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
            <div className="bg-white rounded-2xl shadow-xl w-full max-w-md p-6">
                <div className="flex justify-between items-center mb-4">
                    <h2 className="text-xl font-bold text-gray-900">Propose Swap</h2>
                    <button onClick={onClose} className="text-gray-400 hover:text-gray-600">
                        <X size={20} />
                    </button>
                </div>

                <div className="mb-4 p-3 bg-blue-50 rounded-lg">
                    <p className="text-sm text-gray-500">You want</p>
                    <p className="font-semibold text-gray-900">{targetSkill.title}</p>
                    <p className="text-sm text-gray-500">from {targetSkill.userFullName}</p>
                </div>

                <form onSubmit={handleSubmit} className="space-y-4">
                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">
                            Offer one of your skills in return
                        </label>
                        {offeringSkills.length > 0 ? (
                            <select
                                value={selectedSkillId}
                                onChange={(e) => setSelectedSkillId(Number(e.target.value))}
                                required
                                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 outline-none"
                            >
                                <option value={0} disabled>Select a skill to offer</option>
                                {offeringSkills.map((s) => (
                                    <option key={s.id} value={s.id}>
                                        {s.title} ({s.categoryName})
                                    </option>
                                ))}
                            </select>
                        ) : (
                            <p className="text-sm text-gray-500">
                                You have no skills listed as "Offering". Add one from your Profile first.
                            </p>
                        )}
                    </div>

                    {createSwap.isError && (
                        <p className="text-sm text-red-600">
                            {(createSwap.error as any)?.response?.data?.message || 'Failed to create swap'}
                        </p>
                    )}

                    <button
                        type="submit"
                        disabled={createSwap.isPending || selectedSkillId === 0}
                        className="w-full py-2.5 bg-primary-600 text-white rounded-lg font-medium hover:bg-primary-700 disabled:opacity-50 transition-colors"
                    >
                        {createSwap.isPending ? 'Sending...' : 'Send Swap Request'}
                    </button>
                </form>
            </div>
        </div>
    );
}
