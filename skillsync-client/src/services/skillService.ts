import api from './api';

export interface Skill {
  id: number;
  title: string;
  description?: string;
  proficiencyLevel: string;
  isOffering: boolean;
  categoryName: string;
  categoryId: number;
  userId: string;
  userFullName: string;
  createdAt: string;
}

export interface CreateSkillDto {
  title: string;
  description?: string;
  categoryId: number;
  proficiencyLevel: number;
  isOffering: boolean;
}

export interface SkillQueryParams {
  category?: string;
  search?: string;
  isOffering?: boolean;
  page?: number;
  pageSize?: number;
}

export const skillService = {
  getSkills: (params?: SkillQueryParams) => api.get<Skill[]>('/skills', { params }),
  getSkill: (id: number) => api.get<Skill>(`/skills/${id}`),
  createSkill: (dto: CreateSkillDto) => api.post<Skill>('/skills', dto),
  deleteSkill: (id: number) => api.delete(`/skills/${id}`),
};
