import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { swapService, CreateSwapDto, UpdateSwapStatusDto } from '@/services/swapService';

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
