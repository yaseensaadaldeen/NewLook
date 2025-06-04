$(function () {
    // MENU TOGGLE
    $('.navbar-collapse a').on('click', function () {
        $(".navbar-collapse").collapse('hide');
    });

    // SMOOTHSCROLL NAVBAR
    $('.navbar a, .hero-text a').on('click', function (event) {
        var $anchor = $(this);
        $('html, body').stop().animate({
            scrollTop: $($anchor.attr('href')).offset().top - 49
        }, 1000);
        event.preventDefault();
    });
});

// AOS ANIMATION INITIALIZATION
// Initialize only if AOS elements exist
function initializeAOS() {
    const aosElements = document.querySelectorAll('[data-aos]');
    if (aosElements.length > 0) {
        AOS.init({
            disable: 'mobile',
            duration: 800,
            anchorPlacement: 'center-bottom',
            easing: 'ease-in-out',
            once: true
        });
    }
}

// Initialize everything when DOM is ready
document.addEventListener("DOMContentLoaded", function () {
    initializeAOS();
   // initializeGalleryFilter();
    initializeShoppingCart();
    initializeVideo();
    highlightTodaysHours();
}); 
//DaysStyle
    document.addEventListener("DOMContentLoaded", function () {
    const today = new Date().getDay();
    const items = document.querySelectorAll(".classy-hours ul li");
    items.forEach(item => {
      if (parseInt(item.getAttribute("data-day")) === today) {
        item.classList.add("today");
      }
    });
  });

// GALLERY FILTER FUNCTIONALITY
function initializeGalleryFilter() {
    const filterButtons = document.querySelectorAll('.filter-btn');
    const galleryItems = document.querySelectorAll('.gallery-item');

    function filterGallery(filter) {
        let delay = 0;

        galleryItems.forEach(item => {
            const match = filter === 'all' || item.getAttribute('data-category') === filter;

            setTimeout(() => {
                item.classList.toggle('show', match);
            }, delay);

            if (match) delay += 100;
        });
    }

    filterButtons.forEach(button => {
        button.addEventListener('click', () => {
            const filter = button.getAttribute('data-filter');

            // Update active button
            filterButtons.forEach(btn => btn.classList.remove('active'));
            button.classList.add('active');

            // Filter items
            filterGallery(filter);
        });
    });

    // Initialize all items on page load
    window.addEventListener('DOMContentLoaded', () => {
        galleryItems.forEach((item, i) => {
            setTimeout(() => item.classList.add('show'), i * 100);
        });
    });
}
// VIDEO PLAYBACK FUNCTIONALITY
function initializeVideo() {
    const video = document.querySelector(".hero-video");
    if (!video) return;

    let isReversing = false;

    video.addEventListener("ended", function () {
        if (!isReversing) {
            isReversing = true;
            video.currentTime = video.duration;
            video.play();
        } else {
            isReversing = false;
            video.currentTime = 0;
            video.play();
        }
    });

    video.play();
}

// HIGHLIGHT TODAY'S HOURS
function highlightTodaysHours() {
    const today = new Date().getDay();
    document.querySelectorAll(".classy-hours ul li").forEach(item => {
        if (parseInt(item.getAttribute("data-day")) === today) {
            item.classList.add("today");
        }
    });
}

// NAVBAR BRAND IMAGE LOAD EFFECT
window.addEventListener('load', function () {
    document.querySelector('.navbar-brand img')?.classList.add('loaded');
});

// INITIALIZE ALL FUNCTIONALITY
document.addEventListener("DOMContentLoaded", function () {
    initializeGalleryFilter();
    initializeShoppingCart();
    initializeVideo();
    highlightTodaysHours();
});
document.addEventListener("DOMContentLoaded", function () {
    const today = new Date().getDay();
    const items = document.querySelectorAll(".classy-hours ul li");
    items.forEach(item => {
        if (parseInt(item.getAttribute("data-day")) === today) {
            item.classList.add("today");
        }
    });
});

$(document).ready(function () {
    $('.alert').each(function () {
        const $alert = $(this);
        let dismissTimer;

        const startDismissTimer = () => {
            dismissTimer = setTimeout(() => {
                $alert.addClass('fade-up');
                setTimeout(() => {
                    $alert.alert('close');
                }, 500); // Wait for fade-up animation to finish
            }, 2000); // Start fade-up after 2 second
        };

        const clearDismissTimer = () => {
            clearTimeout(dismissTimer);
            $alert.removeClass('fade-up');
        };

        // Start timer initially
        startDismissTimer();

        // On hover: reset timer and remove fade-up
        $alert.hover(clearDismissTimer, startDismissTimer);
    });
}); function showChoice() {
    document.getElementById("contactChoice").style.display = "block";
    document.getElementById("overlay").style.display = "block";
}

// Close the choice popup and overlay
function closeChoice() {
    document.getElementById("contactChoice").style.display = "none";
    document.getElementById("overlay").style.display = "none";
}

// Get form data values from your inputs (match your actual input names)
function getFormData() {
    const name = document.querySelector('[name="name"]').value;
    const email = document.querySelector('[name="email"]').value;
    const message = document.querySelector('[name="message"]').value;
    return { name, email, message };
}

// Submit form normally via email (actual form submission)
function sendViaEmail() {
    // Optional: validate form here again if you want

    // Submit the form
    document.getElementById("contact-form").submit();

    // Close popup
    closeChoice();
}

// Open WhatsApp chat with the message text
function sendViaWhatsApp() {
    const { name, email, message } = getFormData();
    const text = encodeURIComponent(
        `New Contact Message:\nName: ${name}\nEmail: ${email}\nMessage: ${message}`
    );
    const phone = "971586807722"; // Your WhatsApp number without "+" or spaces
    window.open(`https://wa.me/${phone}?text=${text}`, "_blank");

    closeChoice();
}