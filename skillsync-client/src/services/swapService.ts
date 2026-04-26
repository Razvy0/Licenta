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
  timeSlotStart?: string;
  timeSlotEnd?: string;
  requesterValidated: boolean;
  receiverValidated: boolean;
  createdAt: string;
}

export interface CreateSwapDto {
  offeredSkillId: number;
  requestedSkillId: number;
}

export interface UpdateSwapStatusDto {
  status: string;
}

export interface ProposeTimeSlotDto {
  timeSlotStart: string;
  timeSlotEnd: string;
}

export interface PickTimeDto {
  scheduledDate: string;
}

export const swapService = {
  getSwaps: () => api.get<Swap[]>('/swaps'),
  createSwap: (dto: CreateSwapDto) => api.post<Swap>('/swaps', dto),
  updateStatus: (id: number, dto: UpdateSwapStatusDto) => api.put<Swap>(`/swaps/${id}/status`, dto),
  proposeTimeSlot: (id: number, dto: ProposeTimeSlotDto) => api.put<Swap>(`/swaps/${id}/timeslot`, dto),
  pickTime: (id: number, dto: PickTimeDto) => api.put<Swap>(`/swaps/${id}/pick-time`, dto),
  validate: (id: number) => api.put<Swap>(`/swaps/${id}/validate`),
  invalidate: (id: number) => api.put<Swap>(`/swaps/${id}/invalidate`),
};
