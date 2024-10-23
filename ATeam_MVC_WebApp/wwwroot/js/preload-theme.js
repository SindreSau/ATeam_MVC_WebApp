// set the theme before page load
(function() {
    const getStoredTheme = () => localStorage.getItem('theme');
    const getPreferredTheme = () => {
        const storedTheme = getStoredTheme();
        if (storedTheme) {
            return storedTheme;
        }
        return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
    };

    // Set theme immediately
    const theme = getPreferredTheme();
    document.documentElement.setAttribute('data-bs-theme', theme);

    // Prevent flash by setting background color immediately
    document.documentElement.style.backgroundColor = theme === 'dark' ? '#212529' : '#ffffff';

    // Set the correct icon class before DOM is ready
    document.addEventListener('DOMContentLoaded', () => {
        const icon = document.getElementById('theme-icon');
        if (theme === 'dark') {
            icon.classList.remove('fa-sun');
            icon.classList.add('fa-moon');
        }
        // Show the icon once it's in the correct state
        icon.classList.add('ready');
    });
})();