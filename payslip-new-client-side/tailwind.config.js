export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      height: {
        'screen-minus-100': 'calc(100vh - 100px)',
        // add more if needed
      },
      colors: {
        primary: '#1E1B4B',     // indigo-900 – authority, stability
        secondary: '#F43F5E',   // rose-500 – elegant, bold action color
        muted: '#6B7280',       // gray-500 – for subdued text
        background: '#FAFAFA',  // neutral light – clean, minimal canvas
        accent: '#E0E7FF',      // indigo-100 – subtle highlights or cards
        danger: '#B91C1C',
      },
      fontFamily: {
        sans: ['Vazirmatn', 'Cairo', 'Tajawal', 'Arial', 'sans-serif'],
        cairo: ['Cairo'],
        vazir: ['Vazirmatn', 'sans-serif'],
        en: ['Arial']
      },
    },
  },
  plugins: [],
};
