"use client";

import { SessionProvider } from "next-auth/react";
import { ReactNode } from "react";

interface Props {
  children: ReactNode;
}

export default function Provider({ children }: Props) {
  return <SessionProvider>{children}</SessionProvider>;
}
