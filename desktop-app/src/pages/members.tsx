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
import * as api from '@/lib/api'
import type { Member } from '@/types'
import { ChevronFirst, ChevronLast, Search, User } from 'lucide-react'
import { useEffect, useState } from 'react'

const MAX_BORROWS = 20
const MAX_PAGINATION = 5

type Paginations = {
   page: string | '...'
   active: boolean
   onClick?: () => void
}

export default function Members() {
   const [finalPage, setFinalPage] = useState(0)
   const [member, setMember] = useState<Member | null>(null)
   const [members, setMembers] = useState<Member[]>([])
   const [search, setSearch] = useState('')
   const [pagination, setPagination] = useState<Paginations[]>([])

   useEffect(() => {
      ;(async () => {
         const members = await api.getMembers(1, MAX_BORROWS, search)
         if (members) {
            setMembers(members.data)
            setFinalPage(Math.ceil(members.pagination.totalRecords / MAX_BORROWS))
            setPagination(calculatePagination(1, Math.ceil(members.pagination.totalRecords / MAX_BORROWS)))
         }
      })()
   }, [])

   const onSearch = async (e: React.FormEvent) => {
      e.preventDefault()
      const members = await api.getMembers(1, MAX_BORROWS, search)
      if (members) {
         console.log(members)
         setMembers(members.data)
         setFinalPage(Math.ceil(members.pagination.totalRecords / MAX_BORROWS))
         setPagination(calculatePagination(1, Math.ceil(members.pagination.totalRecords / MAX_BORROWS)))
      }
   }

   const changePage = async (page: number) => {
      const members = await api.getMembers(page, MAX_BORROWS, search)
      if (members) {
         setMembers(members.data)
         setPagination(calculatePagination(page, Math.ceil(members.pagination.totalRecords / MAX_BORROWS)))
      }
   }

   const fistPage = async () => {
      const members = await api.getMembers(1, MAX_BORROWS, search)
      if (members) {
         setMembers(members.data)
         setPagination(calculatePagination(1, Math.ceil(members.pagination.totalRecords / MAX_BORROWS)))
      }
   }

   const lastPage = async () => {
      const members = await api.getMembers(finalPage, MAX_BORROWS, search)
      if (members) {
         setMembers(members.data)
         setPagination(calculatePagination(finalPage, Math.ceil(members.pagination.totalRecords / MAX_BORROWS)))
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
            <div className='grid grid-cols-1 gap-4'>
               {members.map((borrow) => (
                  <div
                     key={borrow.id}
                     className='bg-custom-slate-1 p-2 h-full w-full rounded-lg cursor-pointer hover:bg-custom-hover-accent grid grid-cols-1 justify-around'
                     onClick={() => setMember(borrow)}
                  >
                     <div>
                        <div className='text-lg font-semibold'>{borrow.name}</div>{' '}
                        <div className='text-sm'>{borrow.email}</div>
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
            {member ? (
               <div className='grid place-items-center w-full h-full'>
                  <div className='flex flex-col w-2/3 h-2/3 justify-normal gap-10 items-center'>
                     <div>
                        <span className='font-semibold text-xl'>Member Information:</span>
                        <div>
                           <span className='font-bold'>Name:</span> {member.name}
                           <br />
                           <span className='font-bold'>Email:</span> {member.email}
                        </div>
                     </div>
                     <div>
                        <span className='font-semibold text-xl'>Member Borrowings:</span>
                        <div>
                           {member.borrowings.length > 0 ? (
                              <ul className='list-disc'>
                                 {member.borrowings.map((borrow) => (
                                    <li key={borrow.id}>
                                       <div>
                                          <span className='font-bold'>Book Title:</span> {borrow.bookTitle}
                                       </div>
                                       <div>
                                          <span className='font-bold'>Borrowed Date:</span> {borrow.borrowedDate}
                                       </div>
                                       <div>
                                          <span className='font-bold'>Return Date:</span>{' '}
                                          {borrow.returnDate || 'Not yet returned'}
                                       </div>
                                       <br />
                                    </li>
                                 ))}
                              </ul>
                           ) : (
                              <div className='grid place-items-center'>
                                 <span className='text-custom-dark-grey'>No borrowings</span>
                              </div>
                           )}
                        </div>
                     </div>
                  </div>
               </div>
            ) : (
               <div className='h-full w-full grid place-items-center'>
                  <User className='text-custom-dark-grey w-80 h-80' />
               </div>
            )}
         </div>
      </div>
   )
}
