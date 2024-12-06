import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import {
   Pagination,
   PaginationContent,
   PaginationEllipsis,
   PaginationItem,
   PaginationLink,
} from '@/components/ui/pagination'
import { Skeleton } from '@/components/ui/skeleton'
import * as api from '@/lib/api'
import type { Book } from '@/types'
import { BookDashed, ChevronFirst, ChevronLast, Search } from 'lucide-react'
import { useEffect, useState } from 'react'

const MAX_BOOKS = 20
const MAX_PAGINATION = 5

type Paginations = {
   page: string | '...'
   active: boolean
   onClick?: () => void
}

export default function Books() {
   const [showBook, setShowBook] = useState(false)
   const [finalPage, setFinalPage] = useState(0)
   const [book, setBook] = useState<Book | null>(null)
   const [books, setBooks] = useState<Book[]>([])
   const [search, setSearch] = useState('')
   const [pagination, setPagination] = useState<Paginations[]>([])

   useEffect(() => {
      ;(async () => {
         const books = await api.getBooks(1, MAX_BOOKS, search)
         if (books) {
            setBooks(books.books)
            setFinalPage(Math.ceil(books.totalBooks / MAX_BOOKS))
            setPagination(calculatePagination(1, Math.ceil(books.totalBooks / MAX_BOOKS)))
         }
      })()
   }, [])

   const onSearch = async (e: React.FormEvent) => {
      e.preventDefault()
      const books = await api.getBooks(1, MAX_BOOKS, search)
      if (books) {
         setBooks(books.books)
         setFinalPage(Math.ceil(books.totalBooks / MAX_BOOKS))
         setPagination(calculatePagination(1, Math.ceil(books.totalBooks / MAX_BOOKS)))
      }
   }

   const changePage = async (page: number) => {
      const books = await api.getBooks(page, MAX_BOOKS, search)
      if (books) {
         setBooks(books.books)
         setPagination(calculatePagination(page, Math.ceil(books.totalBooks / MAX_BOOKS)))
      }
   }

   const fistPage = async () => {
      const books = await api.getBooks(1, MAX_BOOKS, search)
      if (books) {
         setBooks(books.books)
         setPagination(calculatePagination(1, Math.ceil(books.totalBooks / MAX_BOOKS)))
      }
   }

   const lastPage = async () => {
      const books = await api.getBooks(finalPage, MAX_BOOKS, search)
      if (books) {
         setBooks(books.books)
         setPagination(calculatePagination(finalPage, Math.ceil(books.totalBooks / MAX_BOOKS)))
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
            <div className='grid grid-cols-4'>
               {books.map((book) => (
                  <div
                     key={book.isbn}
                     className='flex flex-col p-4 items-center rounded-xl cursor-pointer transition-all hover:bg-custom-slate-1 hover:shadow-custom-hover-accent hover:shadow-button'
                     onClick={() => {
                        setBook(book)
                        setShowBook(false)
                     }}
                  >
                     <div className='w-32 h-48 overflow-hidden rounded-md grid place-items-center'>
                        <img
                           src={`https://covers.openlibrary.org/b/isbn/${book.isbn}-M.jpg`}
                           alt={book.title}
                           className='w-full'
                        />
                     </div>
                     <div className='text-center'>
                        <span className='font-medium'>{book.title}</span>
                        <br />
                        <span>{book.author}</span>
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
            {book ? (
               <div className='grid place-items-center w-full h-full'>
                  <div className='flex flex-col items-center p-4 gap-4'>
                     <div className='w-[20rem] h-[28rem] relative overflow-hidden rounded-md grid place-items-center'>
                        {!showBook && <Skeleton className='w-[20rem] h-[28rem] absolute top-1/2 bottom-1/2 bg-custom-slate-1 -translate-y-1/2' />}
                           <img
                              src={`https://covers.openlibrary.org/b/isbn/${book.isbn}-L.jpg`}
                              alt={book.title}
                              className={'object-cover ' + (showBook ? 'opacity-100' : 'opacity-0')}
                              onLoad={() => setShowBook(true)}
                           />
                     </div>
                     <div className='flex justify-center items-start gap-2'>
                        <div className='flex flex-col font-bold items-end'>
                           <span>ISBN:</span>
                           <span>Title:</span>
                           <span>Author:</span>
                           <span>Is Available:</span>
                           <span>Copies Available:</span>
                        </div>
                        <div className='flex flex-col font-semibold'>
                           <span>{book.isbn}</span>
                           <span>{book.title}</span>
                           <span>{book.author}</span>
                           <span>{book.isAvailable ? 'YES' : 'NO'}</span>
                           <span>{book.copiesAvailable}</span>
                        </div>
                     </div>
                  </div>
               </div>
            ) : (
               <div className='h-full w-full grid place-items-center'>
                  <BookDashed className='text-custom-dark-grey w-80 h-80' />
               </div>
            )}
         </div>
      </div>
   )
}
