import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { skillService, SkillQueryParams, CreateSkillDto } from '@/services/skillService';

export function useSkills(params?: SkillQueryParams) {
  return useQuery({
    queryKey: ['skills', params],
    queryFn: () => skillService.getSkills(params).then((r) => r.data),
  });
}

export function useCreateSkill() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (dto: CreateSkillDto) => skillService.createSkill(dto),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['skills'] }),
  });
}
