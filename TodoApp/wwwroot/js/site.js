// Script for handling common functionality across the site

// Show tooltips
document.addEventListener('DOMContentLoaded', function () {
    // Enable tooltips if Bootstrap is loaded
    if (typeof bootstrap !== 'undefined') {
        var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
        var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
            return new bootstrap.Tooltip(tooltipTriggerEl)
        });
    }
});

// Add active class to current nav item
document.addEventListener('DOMContentLoaded', function () {
    const currentLocation = window.location.pathname;
    const navLinks = document.querySelectorAll('.navbar-nav .nav-link');
    
    navLinks.forEach(link => {
        const linkPath = link.getAttribute('href');
        if (linkPath && (currentLocation === linkPath || currentLocation.startsWith(linkPath) && linkPath !== '/')) {
            link.classList.add('active');
        }
    });
});

// Handle form validation errors better
document.addEventListener('DOMContentLoaded', function () {
    // Highlight first field with error
    const firstError = document.querySelector('.field-validation-error');
    if (firstError) {
        const inputField = document.getElementById(firstError.getAttribute('data-valmsg-for'));
        if (inputField) {
            inputField.focus();
        }
    }
});

// Custom error handling
window.addEventListener('error', function (e) {
    console.error('JavaScript error:', e.message);
    // Could send to server or display user-friendly message
    return false;
});
