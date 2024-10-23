(() => {
    const getStoredTheme = () => localStorage.getItem('theme')
    const setStoredTheme = theme => localStorage.setItem('theme', theme)

    const getPreferredTheme = () => {
        const storedTheme = getStoredTheme()
        if (storedTheme) {
            return storedTheme
        }
        return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light'
    }

    const setTheme = theme => {
        document.documentElement.setAttribute('data-bs-theme', theme)
        document.documentElement.style.backgroundColor = theme === 'dark' ? '#212529' : '#ffffff';

        // Update icon
        const icon = document.getElementById('theme-icon')
        if (theme === 'dark') {
            icon.classList.remove('fa-sun')
            icon.classList.add('fa-moon')
        } else {
            icon.classList.remove('fa-moon')
            icon.classList.add('fa-sun')
        }
    }

    // Set initial icon state (since theme was already set in head)
    const currentTheme = getPreferredTheme();
    const icon = document.getElementById('theme-icon')
    if (currentTheme === 'dark') {
        icon.classList.remove('fa-sun')
        icon.classList.add('fa-moon')
    }

    // Add click handler
    document.getElementById('theme-toggle').addEventListener('click', () => {
        const currentTheme = document.documentElement.getAttribute('data-bs-theme')
        const newTheme = currentTheme === 'dark' ? 'light' : 'dark'
        setStoredTheme(newTheme)
        setTheme(newTheme)
    })

    // Listen for system theme changes
    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', () => {
        const storedTheme = getStoredTheme()
        if (storedTheme !== 'light' && storedTheme !== 'dark') {
            setTheme(getPreferredTheme())
        }
    })
})()