type Props = {
   children: React.ReactNode
   props?: React.ButtonHTMLAttributes<HTMLButtonElement>
   isActive: boolean
   onClick: () => void
}

export default function Button({ children, isActive, onClick, ...props }: Props) {
   return (
      <button
         {...props}
         onClick={onClick}
         className={
            'hover:shadow-button h-[4.5rem] w-[4.5rem] grid place-items-center rounded-full transition-all duration-150 ' +
            (isActive
               ? 'bg-custom-active-accent hover:bg-custom-active-accent hover:shadow-custom-active-accent'
               : 'hover:bg-custom-hover-accent hover:shadow-custom-hover-accent')
         }
      >
         {children}
      </button>
   )
}
