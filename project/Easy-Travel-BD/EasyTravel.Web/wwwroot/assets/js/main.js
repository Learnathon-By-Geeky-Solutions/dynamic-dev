/**
* Template Name: Impact
* Updated: Aug 07 2024 with Bootstrap v5.3.3
* Author: BootstrapMade.com
*/

(function () {
    "use strict";

    /**
     * Apply .scrolled class to body when page scrolls
     */
    function toggleScrolled() {
        const body = document.querySelector('body');
        const header = document.querySelector('#header');
        if (!header || !body) return;

        if (!header.classList.contains('scroll-up-sticky') &&
            !header.classList.contains('sticky-top') &&
            !header.classList.contains('fixed-top')) return;

        window.scrollY > 100 ? body.classList.add('scrolled') : body.classList.remove('scrolled');
    }

    window.addEventListener('scroll', toggleScrolled);
    window.addEventListener('load', toggleScrolled);

    /**
     * Mobile nav toggle
     */
    const mobileNavToggleBtn = document.querySelector('.mobile-nav-toggle');

    function mobileNavToggle() {
        const body = document.querySelector('body');
        if (!body || !mobileNavToggleBtn) return;

        body.classList.toggle('mobile-nav-active');
        mobileNavToggleBtn.classList.toggle('bi-list');
        mobileNavToggleBtn.classList.toggle('bi-x');
    }

    if (mobileNavToggleBtn) {
        mobileNavToggleBtn.addEventListener('click', mobileNavToggle);
    }

    /**
     * Close mobile nav on hash links
     */
    document.querySelectorAll('#navmenu a').forEach(link => {
        link.addEventListener('click', () => {
            if (document.body.classList.contains('mobile-nav-active')) {
                mobileNavToggle();
            }
        });
    });

    /**
     * Toggle mobile nav dropdowns
     */
    document.querySelectorAll('.navmenu .toggle-dropdown').forEach(dropdown => {
        dropdown.addEventListener('click', function (e) {
            e.preventDefault();
            const parent = this.parentNode;
            if (!parent) return;

            parent.classList.toggle('active');
            const nextSibling = parent.nextElementSibling;
            if (nextSibling) {
                nextSibling.classList.toggle('dropdown-active');
            }
            e.stopImmediatePropagation();
        });
    });

    /**
     * Preloader removal
     */
    const preloader = document.querySelector('#preloader');
    if (preloader) {
        window.addEventListener('load', () => {
            preloader.remove();
        });
    }

    /**
     * Scroll to top
     */
    const scrollTopBtn = document.querySelector('.scroll-top');

    function toggleScrollTop() {
        if (!scrollTopBtn) return;
        window.scrollY > 100
            ? scrollTopBtn.classList.add('active')
            : scrollTopBtn.classList.remove('active');
    }

    if (scrollTopBtn) {
        scrollTopBtn.addEventListener('click', (e) => {
            e.preventDefault();
            window.scrollTo({ top: 0, behavior: 'smooth' });
        });
    }

    window.addEventListener('load', toggleScrollTop);
    document.addEventListener('scroll', toggleScrollTop);

    /**
     * AOS animation init
     */
    function aosInit() {
        if (typeof AOS !== 'undefined') {
            AOS.init({
                duration: 600,
                easing: 'ease-in-out',
                once: true,
                mirror: false
            });
        }
    }
    window.addEventListener('load', aosInit);

    /**
     * GLightbox init
     */
    if (typeof GLightbox !== 'undefined') {
        GLightbox({ selector: '.glightbox' });
    }

    /**
     * Swiper sliders init
     */
    function initSwiper() {
        if (typeof Swiper === 'undefined') return;

        document.querySelectorAll(".init-swiper").forEach(swiperElement => {
            try {
                const configEl = swiperElement.querySelector(".swiper-config");
                if (!configEl) return;
                const config = JSON.parse(configEl.innerHTML.trim());

                if (swiperElement.classList.contains("swiper-tab")) {
                    if (typeof initSwiperWithCustomPagination === 'function') {
                        initSwiperWithCustomPagination(swiperElement, config);
                    }
                } else {
                    new Swiper(swiperElement, config);
                }
            } catch (err) {
                console.error("Swiper init error:", err);
            }
        });
    }
    window.addEventListener("load", initSwiper);

    /**
     * PureCounter init
     */
    if (typeof PureCounter !== 'undefined') {
        new PureCounter();
    }

    /**
     * Isotope layout & filters
     */
    document.querySelectorAll('.isotope-layout').forEach(isotopeItem => {
        try {
            const layout = isotopeItem.getAttribute('data-layout') ?? 'masonry';
            const filter = isotopeItem.getAttribute('data-default-filter') ?? '*';
            const sort = isotopeItem.getAttribute('data-sort') ?? 'original-order';
            const container = isotopeItem.querySelector('.isotope-container');

            if (!container) return;

            imagesLoaded(container, () => {
                const iso = new Isotope(container, {
                    itemSelector: '.isotope-item',
                    layoutMode: layout,
                    filter: filter,
                    sortBy: sort
                });

                isotopeItem.querySelectorAll('.isotope-filters li').forEach(filterBtn => {
                    filterBtn.addEventListener('click', () => {
                        isotopeItem.querySelector('.filter-active')?.classList.remove('filter-active');
                        filterBtn.classList.add('filter-active');
                        iso.arrange({ filter: filterBtn.getAttribute('data-filter') });
                        aosInit();
                    });
                });
            });
        } catch (err) {
            console.error("Isotope init error:", err);
        }
    });

    /**
     * FAQ toggle
     */
    document.querySelectorAll('.faq-item h3, .faq-item .faq-toggle').forEach(el => {
        el.addEventListener('click', () => {
            el.closest('.faq-item')?.classList.toggle('faq-active');
        });
    });

    /**
     * Scroll to hash on load
     */
    window.addEventListener('load', () => {
        if (window.location.hash) {
            const target = document.querySelector(window.location.hash);
            if (target) {
                setTimeout(() => {
                    const scrollMarginTop = parseInt(getComputedStyle(target).scrollMarginTop || 0);
                    window.scrollTo({
                        top: target.offsetTop - scrollMarginTop,
                        behavior: 'smooth'
                    });
                }, 100);
            }
        }
    });

    /**
     * Navmenu Scrollspy
     */
    const navLinks = document.querySelectorAll('.navmenu a');

    function navmenuScrollspy() {
        const scrollPos = window.scrollY + 200;

        navLinks.forEach(link => {
            if (!link.hash) return;

            const section = document.querySelector(link.hash);
            if (!section) return;

            if (scrollPos >= section.offsetTop && scrollPos <= section.offsetTop + section.offsetHeight) {
                document.querySelectorAll('.navmenu a.active').forEach(l => l.classList.remove('active'));
                link.classList.add('active');
            } else {
                link.classList.remove('active');
            }
        });
    }

    window.addEventListener('load', navmenuScrollspy);
    document.addEventListener('scroll', navmenuScrollspy);
})();
