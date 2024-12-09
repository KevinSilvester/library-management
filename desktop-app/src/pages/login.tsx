import { Button } from '@/components/ui/button'
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from '@/components/ui/form'
import { Input } from '@/components/ui/input'
import { zodResolver } from '@hookform/resolvers/zod'
import { useForm } from 'react-hook-form'
import { z } from 'zod'

const FormSchema = z.object({
   username: z.string().min(2, {
      message: 'Username must be at least 2 characters.'
   }),
   password: z.string().min(4, {
      message: 'Password must be at least 4 characters.'
   })
})

type Props = {
   login: (username: string, password: string) => Promise<void>
}

export default function Login({ login }: Props) {
   const form = useForm<z.infer<typeof FormSchema>>({
      resolver: zodResolver(FormSchema),
      defaultValues: {
         username: '',
         password: ''
      }
   })

   return (
      <div className='grid place-items-center w-full h-full'>
         <Form {...form}>
            <form
               onSubmit={form.handleSubmit(async (user) => await login(user.username, user.password))}
               className='w-[30rem] space-y-6 bg-custom-bg-primary p-10 rounded-xl'
            >
               <FormField
                  control={form.control}
                  name='username'
                  render={({ field }) => (
                     <FormItem>
                        <FormLabel>Username</FormLabel>
                        <FormControl>
                           <Input className='rounded-xl' placeholder='username' {...field} />
                        </FormControl>
                        <FormMessage />
                     </FormItem>
                  )}
               />
               <FormField
                  control={form.control}
                  name='password'
                  render={({ field }) => (
                     <FormItem>
                        <FormLabel>Password</FormLabel>
                        <FormControl>
                           <Input className='rounded-xl' type='password' placeholder='password' {...field} />
                        </FormControl>
                        <FormMessage />
                     </FormItem>
                  )}
               />
               <Button type='submit'>Login</Button>
            </form>
         </Form>
      </div>
   )
}
