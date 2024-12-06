import icon from '@/assets/icon.png'

type Props = {
   className?: string
   props?: React.ImgHTMLAttributes<HTMLImageElement>
}

export default function Logo({ className, ...props}: Props) {
   return (
      <>
         <img src={icon} alt='Logo' {...props} className={className}/>
      </>
   )
}
