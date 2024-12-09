import { Book, BooksRes, BorrowsRes, MembersRes } from '@/types'
import { fetch } from '@tauri-apps/plugin-http'

export async function login(username: string, password: string) {
   try {
      const res = await fetch('https://library-management-production.up.railway.app/api/Auth/login', {
         method: 'POST',
         headers: { 'Content-Type': 'application/json' },
         body: JSON.stringify({ username, password })
      })
      window._cookie = res.headers.getSetCookie()[0]
      if (res.ok) {
         return true
      }
   } catch (e) {
      console.log(e)
   }
   return false
}

function setHeaders(): HeadersInit {
   return {
      Cookie: window._cookie,
      Accept: 'application/json'
   } as HeadersInit
}

export async function logout() {
   try {
      const res = await fetch('https://library-management-production.up.railway.app/api/Auth/logout', {
         method: 'POST',
         headers: setHeaders()
      })
      if (res.ok) {
         window._cookie = null
         return true
      }
   } catch (e) {
      console.log(e)
   }
   window._cookie = null
}

export async function getBooks(page: number, pageSize: number, search: string): Promise<BooksRes | null> {
   try {
      const res = await fetch(`https://library-management-production.up.railway.app/api/Books?search=${search}&page=${page}&pageSize=${pageSize}`, {
         method: 'GET',
         headers: setHeaders()
      })

      if (res.ok) {
         return await JSON.parse(await res.text())
      }
   } catch (e) {
      console.log(e)
   }
   return null
}

export async function getBook(isbn: string): Promise<Book | null> {
   try {
      const res = await fetch(`https://library-management-production.up.railway.app/api/Books/${isbn}`, {
         method: 'GET',
         headers: setHeaders()
      })

      if (res.ok) {
         return await JSON.parse(await res.text())
      }
   } catch (e) {
      console.log(e)
   }
   return null
}

export async function getBorrows(pageNumber: number, pageSize: number, search: string): Promise<BorrowsRes | null> {
   try {
      const res = await fetch(
         `https://library-management-production.up.railway.app/api/Borrowings?query=${search}&pageNumber=${pageNumber}&pageSize=${pageSize}`,
         {
            method: 'GET',
            headers: setHeaders()
         }
      )

      if (res.ok) {
         return await JSON.parse(await res.text())
      }
   } catch (e) {
      console.log(e)
   }
   return null
}

export async function getMembers(pageNumber: number, pageSize: number, search: string): Promise<MembersRes | null> {
   try {
      const res = await fetch(
         `https://library-management-production.up.railway.app/api/Members?query=${search}&pageNumber=${pageNumber}&pageSize=${pageSize}`,
         {
            method: 'GET',
            headers: setHeaders()
         }
      )

      if (res.ok) {
         return await JSON.parse(await res.text())
      }
   } catch (e) {
      console.log(e)
   }
   return null
}
