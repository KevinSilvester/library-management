import { create } from 'zustand'
import { immer } from 'zustand/middleware/immer'

type Tabs = 'home' | 'checkout' | 'borrowings' | 'settings'


type State = {
   pageNum: number
}

type Actions = {
   setPageNum: (num: number) => void
}

export const useStore = create<State & Actions>()(
   immer((set) => ({
      pageNum: 1,
      setPageNum: (num: number) => set((state) => (state.pageNum = num))
   }))
)
