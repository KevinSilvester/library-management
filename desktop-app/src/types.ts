export interface UserData {
   id: number
   name: string
   email: string
   apiKey: string
   accessLevel: 'STAFF' | 'MEMBER'
}

export type UserList = { id: number; name: string }[]
