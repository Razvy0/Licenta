import { useState } from 'react';
import { useSkills } from '@/hooks/useSkills';
import SkillCard from '@/components/SkillCard';
import { Search } from 'lucide-react';

export default function ExplorePage() {
  const [search, setSearch] = useState('');
  const [offeringFilter, setOfferingFilter] = useState<boolean | undefined>(true);
  const { data: skills, isLoading } = useSkills({
    search: search || undefined,
    isOffering: offeringFilter,
  });

  return (
    <div>
      <h1 className="text-2xl font-bold text-gray-900 mb-6">Explore Skills</h1>

      <div className="flex gap-4 mb-6">
        <div className="relative flex-1">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400" size={18} />
          <input
            type="text"
            placeholder="Search skills..."
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent outline-none"
          />
        </div>
        <select
          value={offeringFilter === undefined ? 'all' : offeringFilter ? 'offering' : 'seeking'}
          onChange={(e) => {
            const v = e.target.value;
            setOfferingFilter(v === 'all' ? undefined : v === 'offering');
          }}
          className="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 outline-none"
        >
          <option value="all">All</option>
          <option value="offering">Offering</option>
          <option value="seeking">Seeking</option>
        </select>
      </div>

      {isLoading ? (
        <p className="text-gray-500">Loading skills...</p>
      ) : skills && skills.length > 0 ? (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {skills.map((skill) => (
            <SkillCard key={skill.id} skill={skill} showActions />
          ))}
        </div>
      ) : (
        <p className="text-gray-500">No skills found.</p>
      )}
    </div>
  );
}
