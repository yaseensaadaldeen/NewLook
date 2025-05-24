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

// LIGHTBOX INITIALIZATION
const lightbox = GLightbox({
    selector: '.glightbox',
    touchNavigation: true,
    loop: true,
    zoomable: true
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

// SHOPPING CART FUNCTIONALITY
function initializeShoppingCart() {
    const chooseButtons = document.querySelectorAll('.choose-service');
    const cartSection = document.getElementById('cart-section');
    const cartItemsContainer = document.getElementById('cart-items');
    const totalCostElement = document.getElementById('total-cost');
    const clearCartButton = document.getElementById('clear-cart');
    const minimizeCartButton = document.getElementById('minimize-cart');
    const showCart = document.getElementById('show-cart');
    const showCartTotal = document.getElementById('show-cart-total');
    const openCartButton = document.getElementById('open-cart-button');

    let cartItems = [];
    let totalCost = 0;

    function openCart() {
        cartSection.style.display = 'block';
        showCart.style.display = 'none';
    }

    function closeCart() {
        cartSection.style.display = 'none';
        if (cartItems.length > 0) {
            showCart.style.display = 'block';
        }
    }

    function updateCart() {
        cartItemsContainer.innerHTML = '';
        cartItems.forEach((item, index) => {
            const itemElement = document.createElement('div');
            itemElement.classList.add('service-item');
            itemElement.innerHTML = `
                <span>${item.name}</span>
                <span>$${item.cost}</span>
                <button class="remove-item btn btn-sm btn-danger" data-index="${index}">&times;</button>
            `;
            cartItemsContainer.appendChild(itemElement);
        });

        totalCost = cartItems.reduce((sum, item) => sum + item.cost, 0);
        totalCostElement.innerText = totalCost.toFixed(2);
        showCartTotal.innerText = `$${totalCost.toFixed(2)}`;

        // Add event listeners to remove buttons
        document.querySelectorAll('.remove-item').forEach(button => {
            button.addEventListener('click', function () {
                const index = parseInt(this.dataset.index);
                cartItems.splice(index, 1);
                updateCart();
                if (cartItems.length === 0) {
                    closeCart();
                    showCart.style.display = 'none';
                }
            });
        });
    }

    // Event Listeners
    chooseButtons.forEach(button => {
        button.addEventListener('click', function () {
            const serviceName = this.dataset.name;
            const serviceCost = parseFloat(this.dataset.cost);

            cartItems.push({ name: serviceName, cost: serviceCost });
            updateCart();
            openCart();
        });
    });

    clearCartButton.addEventListener('click', function () {
        cartItems = [];
        totalCost = 0;
        updateCart();
        closeCart();
    });

    minimizeCartButton.addEventListener('click', closeCart);
    showCart.addEventListener('click', openCart);
    openCartButton?.addEventListener('click', function (e) {
        e.preventDefault();
        openCart();
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
const lightbox = GLightbox({
    selector: '.glightbox',
    touchNavigation: true,
    loop: true,
    zoomable: true,
    closeButton: true
});
