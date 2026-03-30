import api from './api';

export interface Swap {
  id: number;
  requesterId: string;
  requesterName: string;
  receiverId: string;
  receiverName: string;
  offeredSkillTitle: string;
  requestedSkillTitle: string;
  status: string;
  scheduledDate?: string;
  createdAt: string;
}

export interface CreateSwapDto {
  offeredSkillId: number;
  requestedSkillId: number;
  scheduledDate?: string;
}

export interface UpdateSwapStatusDto {
  status: number;
}

export const swapService = {
  getSwaps: () => api.get<Swap[]>('/swaps'),
  createSwap: (dto: CreateSwapDto) => api.post<Swap>('/swaps', dto),
  updateStatus: (id: number, dto: UpdateSwapStatusDto) => api.put<Swap>(`/swaps/${id}/status`, dto),
};
