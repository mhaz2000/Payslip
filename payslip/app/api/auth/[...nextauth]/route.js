import NextAuth from "next-auth";
import CredentialsProvider from "next-auth/providers/credentials";

const handler = NextAuth({
  secret: process.env.NEXTAUTH_SECRET,
  pages: {
    signIn: "/login",
    signOut: "/login",
  },
  providers: [
    CredentialsProvider({
      // The name to display on the sign in form (e.g. 'Sign in with...')
      name: "Credentials",
      credentials: {
        username: {},
        password: {},
      },
      async authorize(credentials, req)
      {

        try
        {
          const res = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/api/authentication/login`, {
            method: "POST",
            body: JSON.stringify({
              username: credentials?.username,
              password: credentials?.password,
            }),
            headers: { "Content-Type": "application/json" },
          });

          const data = await res.json();

          if (res.ok && data)
          {
            return data;
          }

          return null;
        } catch (error)
        {
          console.log(error)
          return null
        }
      },
    }),
  ],
  callbacks: {
    async jwt({ token, user, session })
    {
      return { ...token, ...user };
    },
    async session({ session, token, user })
    {
      session.user = token;
      session.user.name = token['isAdmin'] ? 'admin' : '';
      session.user.mustChangePassword = token['mustChangePassword'];
      session.mustChangePassword = token['mustChangePassword'];

      console.log(session)
      return session;
    },
  },
});

export { handler as GET, handler as POST };
