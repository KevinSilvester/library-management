import { fetch } from '@tauri-apps/plugin-http'
import { BookUp2, LibraryBig, LogOut, Users } from 'lucide-react'
import { useEffect, useState } from 'react'

import Button from './components/button'
import Logo from './components/logo'
import { Toaster } from './components/ui/toaster'
import { useToast } from './hooks/use-toast'
import * as api from './lib/api'
import Books from './pages/books'
import Borrows from './pages/borrows'
import Login from './pages/login'
import Members from './pages/members'

type Page = 'books' | 'borrows' | 'members'

function SelectedPage({ pageName }: { pageName: Page }) {
   switch (pageName) {
      case 'books':
         return <Books />
      case 'borrows':
         return <Borrows />
      case 'members':
         return <Members />
   }
}

function App() {
   // const [userList, setUserList] = useState<UserList>([])
   // const [user, setUser] = useState<UserList>([])
   // useEffect(() => {
   //    ;(async () => setUserList(await invoke('list_users')))()
   // }, [])

   const [isLoggedIn, setIsLoggedIn] = useState(false)
   const [activePageName, setActiveTabName] = useState<Page>('books')
   const { toast } = useToast()

   const login = async (username: string, password: string) => {
      // if (await api.login(username, password)) {
      if (await api.login('admin', 'test123')) {
         setIsLoggedIn(true)
         toast({
            duration: 1000,
            variant: 'default',
            title: 'Login Successful'
         })
         return
      }

      toast({
         duration: 1000,
         variant: 'destructive',
         title: 'Login Failed',
         description: 'Invalid username or password'
      })
   }

   const logout = async () => {
      if (!isLoggedIn) {
         toast({
            duration: 1000,
            variant: 'destructive',
            title: 'Login First'
         })
         return
      }

      if (await api.logout()) {
         setIsLoggedIn(false)
         toast({
            duration: 1000,
            variant: 'default',
            title: 'Logged Out'
         })
         return
      }

      toast({
         duration: 1000,
         variant: 'destructive',
         title: 'Logout Failed'
      })
   }

   const switchTab = (page: Page) => () => {
      if (!isLoggedIn) {
         toast({
            duration: 1000,
            variant: 'destructive',
            title: 'Login First',
            description: 'This page can only be viewed after logging in'
         })
         return
      }

      if (page === activePageName) return
      setActiveTabName(page)
   }

   useEffect(() => {}, [])

   return (
      <div className='h-screen w-full'>
         <Toaster />
         <div className='flex flex-row h-full w-full'>
            <div className='flex flex-col justify-around w-24 h-full relative bg-custom-bg-primary py-16'>
               {/* Logo */}
               <div className='w-full grid place-items-center -translate-y-5'>
                  <div className='h-20 w-20 grid place-items-center'>
                     <Logo className='h-full' />
                     <span className='text-center font-bold text-custom-dark-grey'>Library App</span>
                  </div>
               </div>

               {/* Nav Buttons */}
               <div className='h-full py-36 flex flex-col items-center justify-evenly -translate-y-8'>
                  <div className='text-center'>
                     <Button isActive={isLoggedIn && activePageName === 'books'} onClick={switchTab('books')}>
                        <LibraryBig className='text-custom-dark-grey' />
                     </Button>
                     <span className='font-semibold'>Books</span>
                  </div>
                  <div className='text-center'>
                     <Button isActive={isLoggedIn && activePageName === 'borrows'} onClick={switchTab('borrows')}>
                        <BookUp2 className='text-custom-dark-grey' />
                     </Button>
                     <span className='font-semibold'>Borrows</span>
                  </div>
                  <div className='text-center'>
                     <Button isActive={isLoggedIn && activePageName === 'members'} onClick={switchTab('members')}>
                        <Users className='text-custom-dark-grey' />
                     </Button>
                     <span className='font-semibold'>Members</span>
                  </div>
               </div>

               {/* Logout Button */}
               <div className='w-full grid place-items-center'>
                  <Button isActive={false} onClick={logout}>
                     <LogOut className='text-custom-dark-grey' />
                  </Button>
                  <span className='font-semibold'>Logout</span>
               </div>

               {/* Divider */}
               <div className='absolute h-[75%] bg-custom-slate-1 w-[3px] top-[50%] translate-y-[-50%] translate-x-[50%] right-0 rounded-lg' />
            </div>

            {/* Main Content */}
            {isLoggedIn ? <SelectedPage pageName={activePageName} /> : <Login login={login} />}
         </div>
      </div>
   )
}

export default App
