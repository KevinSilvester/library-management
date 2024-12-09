import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import {
   Pagination,
   PaginationContent,
   PaginationEllipsis,
   PaginationItem,
   PaginationLink
} from '@/components/ui/pagination'
import { Skeleton } from '@/components/ui/skeleton'
import * as api from '@/lib/api'
import type { Borrow } from '@/types'
import { BookUp2, ChevronFirst, ChevronLast, Search } from 'lucide-react'
import { useEffect, useState } from 'react'

const MAX_BORROWS = 20
const MAX_PAGINATION = 5

type Paginations = {
   page: string | '...'
   active: boolean
   onClick?: () => void
}

export default function Borrows() {
   const [showBorrow, setShowBorrow] = useState(false)
   const [finalPage, setFinalPage] = useState(0)
   const [borrow, setBorrow] = useState<Borrow | null>(null)
   const [borrows, setBorrows] = useState<Borrow[]>([])
   const [search, setSearch] = useState('')
   const [pagination, setPagination] = useState<Paginations[]>([])

   useEffect(() => {
      ;(async () => {
         const borrows = await api.getBorrows(1, MAX_BORROWS, search)
         if (borrows) {
            setBorrows(borrows.data)
            setFinalPage(Math.ceil(borrows.pagination.totalRecords / MAX_BORROWS))
            setPagination(calculatePagination(1, Math.ceil(borrows.pagination.totalRecords / MAX_BORROWS)))
         }
      })()
   }, [])

   const onSearch = async (e: React.FormEvent) => {
      e.preventDefault()
      const borrows = await api.getBorrows(1, MAX_BORROWS, search)
      if (borrows) {
         setBorrows(borrows.data)
         setFinalPage(Math.ceil(borrows.pagination.totalRecords / MAX_BORROWS))
         setPagination(calculatePagination(1, Math.ceil(borrows.pagination.totalRecords / MAX_BORROWS)))
      }
   }

   const changePage = async (page: number) => {
      const borrows = await api.getBorrows(page, MAX_BORROWS, search)
      if (borrows) {
         setBorrows(borrows.data)
         setPagination(calculatePagination(page, Math.ceil(borrows.pagination.totalRecords / MAX_BORROWS)))
      }
   }

   const fistPage = async () => {
      const borrows = await api.getBorrows(1, MAX_BORROWS, search)
      if (borrows) {
         setBorrows(borrows.data)
         setPagination(calculatePagination(1, Math.ceil(borrows.pagination.totalRecords / MAX_BORROWS)))
      }
   }

   const lastPage = async () => {
      const borrows = await api.getBorrows(finalPage, MAX_BORROWS, search)
      if (borrows) {
         setBorrows(borrows.data)
         setPagination(calculatePagination(finalPage, Math.ceil(borrows.pagination.totalRecords / MAX_BORROWS)))
      }
   }

   const calculatePagination = (page: number, finalPage: number) => {
      const paginations: Paginations[] = []
      if (finalPage <= MAX_PAGINATION) {
         for (let i = 1; i <= finalPage; i++) {
            const active = i === page
            const onClick = active ? undefined : () => changePage(i)
            paginations.push({ page: i.toString(), active: i === page, onClick })
         }
      } else {
         if (page <= 3) {
            for (let i = 1; i <= 5; i++) {
               const active = i === page
               const onClick = active ? undefined : () => changePage(i)
               paginations.push({ page: i.toString(), active: i === page, onClick })
            }
            paginations.push({ page: '...', active: false, onClick: () => changePage(page + 1) })
            paginations.push({ page: finalPage.toString(), active: false, onClick: () => changePage(finalPage) })
         } else if (page >= finalPage - 2) {
            paginations.push({ page: '1', active: false, onClick: () => changePage(1) })
            paginations.push({ page: '...', active: false, onClick: () => changePage(page - 1) })
            for (let i = finalPage - 4; i <= finalPage; i++) {
               const active = i === page
               const onClick = active ? undefined : () => changePage(i)
               paginations.push({ page: i.toString(), active: i === page, onClick })
            }
         } else {
            paginations.push({ page: '1', active: false, onClick: () => changePage(1) })
            paginations.push({ page: '...', active: false, onClick: () => changePage(page - 1) })
            for (let i = page - 1; i <= page + 1; i++) {
               const active = i === page
               const onClick = active ? undefined : () => changePage(i)
               paginations.push({ page: i.toString(), active: i === page, onClick })
            }
            paginations.push({ page: '...', active: false, onClick: () => changePage(page + 1) })
            paginations.push({
               page: finalPage.toString(),
               active: false,
               onClick: () => changePage(finalPage)
            })
         }
      }
      return paginations
   }

   return (
      <div className='w-full flex overflow-hidden'>
         <div className='flex-[3] bg-custom-bg-primary overflow-y-auto p-2'>
            <form onSubmit={onSearch} className='w-full grid place-items-center h-20 mb-3'>
               <Label htmlFor='search' className='text-lg'>
                  Search
               </Label>
               <div className='flex gap-2'>
                  <Input
                     value={search}
                     onChange={(e) => setSearch(e.currentTarget.value)}
                     id='search'
                     className='w-96'
                     placeholder='Search...'
                  />
                  <Button className='rounded-lg' type='submit'>
                     <Search />
                  </Button>
               </div>
            </form>
            <div className='grid grid-cols-3 gap-4'>
               {borrows.map((borrow) => (
                  <div
                     key={borrow.id}
                     className='bg-custom-slate-1 p-2 h-full w-full rounded-lg cursor-pointer hover:bg-custom-hover-accent grid grid-cols-2 justify-around'
                     onClick={() => {
                        setShowBorrow(false)
                        setBorrow(borrow)
                     }}
                  >
                     <div>
                        <div className='text-lg font-semibold'>{borrow.bookTitle}</div>
                        <div className='text-sm'>{borrow.memberName}</div>
                        <div className='text-sm'>{borrow.borrowedDate}</div>
                        <div className='text-sm'>{borrow.returnDate}</div>
                     </div>
                     <div className='h-full w-full grid place-items-center'>
                        <img
                           src={`https://covers.openlibrary.org/b/isbn/${borrow.bookISBN}-S.jpg`}
                           alt='cover'
                           className='h-20'
                        />
                     </div>
                  </div>
               ))}
            </div>
            <div className='mt-4 grid h-10 place-items-center w-full'>
               <Pagination>
                  <PaginationContent>
                     <PaginationItem>
                        <PaginationLink
                           onClick={fistPage}
                           className='bg-custom-slate-1 hover:bg-custom-hover-accent cursor-pointer'
                        >
                           <ChevronFirst />
                        </PaginationLink>
                     </PaginationItem>

                     {pagination.map((p, idx) => (
                        <PaginationItem key={p.page + idx}>
                           {p.page === '...' ? (
                              <PaginationEllipsis className='bg-custom-slate-1' />
                           ) : (
                              <PaginationLink
                                 className={
                                    p.active
                                       ? 'bg-custom-active-accent hover:bg-custom-active-accent'
                                       : 'bg-custom-slate-1 hover:bg-custom-hover-accent cursor-pointer'
                                 }
                                 onClick={p.onClick}
                              >
                                 {p.page}
                              </PaginationLink>
                           )}
                        </PaginationItem>
                     ))}

                     <PaginationItem>
                        <PaginationLink
                           onClick={lastPage}
                           className='bg-custom-slate-1 hover:bg-custom-hover-accent cursor-pointer'
                        >
                           <ChevronLast />
                        </PaginationLink>
                     </PaginationItem>
                  </PaginationContent>
               </Pagination>
            </div>
         </div>
         <div className='flex-[2] bg-custom-bg-secondary'>
            {borrow ? (
               <div className='grid place-items-center w-full h-full'>
                  <div className='flex flex-col items-center p-4 gap-4'>
                     <div className='w-[20rem] h-[28rem] relative overflow-hidden rounded-md grid place-items-center'>
                        {!showBorrow && (
                           <Skeleton className='w-[20rem] h-[28rem] absolute top-1/2 bottom-1/2 bg-custom-slate-1 -translate-y-1/2' />
                        )}
                        <img
                           src={`https://covers.openlibrary.org/b/isbn/${borrow.bookISBN}-L.jpg`}
                           alt={borrow.bookTitle}
                           className={'object-cover ' + (showBorrow ? 'opacity-100' : 'opacity-0')}
                           onLoad={() => setShowBorrow(true)}
                        />
                     </div>
                     <div className='flex justify-center items-start gap-2'>
                        <div className='flex flex-col font-bold items-end'>
                           <span>ISBN:</span>
                           <span>Title:</span>
                           <span>Member Name:</span>
                           <span>Borrowed Date:</span>
                           <span>Returned Date:</span>
                        </div>
                        <div className='flex flex-col font-semibold'>
                           <span>{borrow.bookISBN}</span>
                           <span>{borrow.bookTitle}</span>
                           <span>{borrow.memberName}</span>
                           <span>{borrow.borrowedDate}</span>
                           <span>{borrow.returnDate || 'NOT RETURNED'}</span>
                        </div>
                     </div>
                  </div>
               </div>
            ) : (
               <div className='h-full w-full grid place-items-center'>
                  <BookUp2 className='text-custom-dark-grey w-80 h-80' />
               </div>
            )}
         </div>
      </div>
   )
}
