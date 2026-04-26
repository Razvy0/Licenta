import { useState } from 'react';
import { useCreateSkill } from '@/hooks/useSkills';
import { useCategories } from '@/hooks/useCategories';
import { X } from 'lucide-react';

interface Props {
    onClose: () => void;
}

const proficiencyLevels = ['Beginner', 'Intermediate', 'Advanced', 'Expert'];

export default function AddSkillModal({ onClose }: Props) {
    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');
    const [categoryId, setCategoryId] = useState<number>(0);
    const [proficiencyLevel, setProficiencyLevel] = useState(0);
    const [isOffering, setIsOffering] = useState(true);

    const { data: categories } = useCategories();
    const createSkill = useCreateSkill();

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        createSkill.mutate(
            { title, description: description || undefined, categoryId, proficiencyLevel, isOffering },
            { onSuccess: () => onClose() }
        );
    };

    return (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
            <div className="bg-white rounded-2xl shadow-xl w-full max-w-md p-6">
                <div className="flex justify-between items-center mb-4">
                    <h2 className="text-xl font-bold text-gray-900">Add Skill</h2>
                    <button onClick={onClose} className="text-gray-400 hover:text-gray-600">
                        <X size={20} />
                    </button>
                </div>

                <form onSubmit={handleSubmit} className="space-y-4">
                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">Title</label>
                        <input
                            type="text"
                            value={title}
                            onChange={(e) => setTitle(e.target.value)}
                            required
                            className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent outline-none"
                            placeholder="e.g. React Development"
                        />
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">Description</label>
                        <textarea
                            value={description}
                            onChange={(e) => setDescription(e.target.value)}
                            rows={3}
                            className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent outline-none resize-none"
                            placeholder="Describe what you can teach or want to learn..."
                        />
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">Category</label>
                        <select
                            value={categoryId}
                            onChange={(e) => setCategoryId(Number(e.target.value))}
                            required
                            className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 outline-none"
                        >
                            <option value={0} disabled>Select a category</option>
                            {categories?.map((c) => (
                                <option key={c.id} value={c.id}>{c.name}</option>
                            ))}
                        </select>
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">Proficiency</label>
                        <select
                            value={proficiencyLevel}
                            onChange={(e) => setProficiencyLevel(Number(e.target.value))}
                            className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 outline-none"
                        >
                            {proficiencyLevels.map((level, i) => (
                                <option key={level} value={i}>{level}</option>
                            ))}
                        </select>
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-1">Type</label>
                        <div className="flex gap-4">
                            <label className="flex items-center gap-2 cursor-pointer">
                                <input
                                    type="radio"
                                    checked={isOffering}
                                    onChange={() => setIsOffering(true)}
                                    className="text-primary-600"
                                />
                                <span className="text-sm">Offering</span>
                            </label>
                            <label className="flex items-center gap-2 cursor-pointer">
                                <input
                                    type="radio"
                                    checked={!isOffering}
                                    onChange={() => setIsOffering(false)}
                                    className="text-primary-600"
                                />
                                <span className="text-sm">Seeking</span>
                            </label>
                        </div>
                    </div>

                    {createSkill.isError && (
                        <p className="text-sm text-red-600">
                            {(createSkill.error as any)?.response?.data?.message || 'Failed to create skill'}
                        </p>
                    )}

                    <button
                        type="submit"
                        disabled={createSkill.isPending || categoryId === 0}
                        className="w-full py-2.5 bg-primary-600 text-white rounded-lg font-medium hover:bg-primary-700 disabled:opacity-50 transition-colors"
                    >
                        {createSkill.isPending ? 'Adding...' : 'Add Skill'}
                    </button>
                </form>
            </div>
        </div>
    );
}
