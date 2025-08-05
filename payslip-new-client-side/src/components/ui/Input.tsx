// src/components/ui/Input.tsx
import { forwardRef, type InputHTMLAttributes } from 'react';
import { cn } from '../../utils/cn';

type InputProps = InputHTMLAttributes<HTMLInputElement> & {
  label?: string;
  error?: string;
  labelColor?: string;
};

const Input = forwardRef<HTMLInputElement, InputProps>(({ label, labelColor, error, className, ...props }, ref) => {
  return (
    <div className="w-full space-y-1">
      {label && <label className={`block text-sm font-medium ${labelColor}`}>{label}</label>}
      <input
        ref={ref}
        className={cn(
          'w-full px-3 py-2 rounded-md border shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 transition',
          error ? 'border-red-500' : 'border-gray-300',
          className
        )}
        {...props}
      />
      {error && <p className="text-sm text-red-600">{error}</p>}
    </div>
  );
});

export default Input;
