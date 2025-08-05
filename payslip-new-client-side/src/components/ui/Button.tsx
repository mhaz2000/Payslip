// src/components/ui/Button.tsx
import { type ButtonHTMLAttributes } from 'react';
import { cn } from '../../utils/cn';

type ButtonProps = ButtonHTMLAttributes<HTMLButtonElement> & {
  isLoading?: boolean;
};

const Button = ({ children, isLoading, className, ...props }: ButtonProps) => {
  return (
    <button
      className={cn(
        'inline-flex items-center justify-center px-4 py-2 rounded-md transition disabled:opacity-50 disabled:cursor-not-allowed',
        className
      )}
      disabled={isLoading || props.disabled}
      {...props}
    >
      {isLoading ? 'Loading...' : children}
    </button>
  );
};

export default Button;
