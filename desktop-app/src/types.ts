export interface Book {
   isbn: string;
   title: string
   author: string
   copiesAvailable: number
   isAvailable: boolean
}

export interface BooksRes {
   totalBooks: number
   page: number
   pageSize: number
   books: Book[]
}

export interface Borrow {
   id: number
   bookIsbn: string
   bookTitle: string
   memberName: string
   borrowedDate: string
   returnDate?: string
}

export interface Member {
   id: number
   name: string
   email: string
   borrowings: Borrow[]
}
