import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { swapService, CreateSwapDto, UpdateSwapStatusDto, ProposeTimeSlotDto, PickTimeDto } from '@/services/swapService';

export function useSwaps() {
  return useQuery({
    queryKey: ['swaps'],
    queryFn: () => swapService.getSwaps().then((r) => r.data),
  });
}

export function useCreateSwap() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (dto: CreateSwapDto) => swapService.createSwap(dto),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['swaps'] }),
  });
}

export function useUpdateSwapStatus() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: ({ id, dto }: { id: number; dto: UpdateSwapStatusDto }) =>
      swapService.updateStatus(id, dto),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['swaps'] }),
  });
}

export function useProposeTimeSlot() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: ({ id, dto }: { id: number; dto: ProposeTimeSlotDto }) =>
      swapService.proposeTimeSlot(id, dto),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['swaps'] }),
  });
}

export function usePickTime() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: ({ id, dto }: { id: number; dto: PickTimeDto }) =>
      swapService.pickTime(id, dto),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['swaps'] }),
  });
}

export function useValidateSwap() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: number) => swapService.validate(id),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['swaps'] }),
  });
}

export function useInvalidateSwap() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: number) => swapService.invalidate(id),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['swaps'] }),
  });
}
