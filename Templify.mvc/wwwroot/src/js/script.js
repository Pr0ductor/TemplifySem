document.addEventListener('DOMContentLoaded', function() {
  const burger = document.querySelector('.header__burger');
  const sidebar = document.querySelector('.sidebar');
  const overlay = document.querySelector('.overlay');

  // --- sidebar logic ---
  function openSidebar() {
    sidebar.classList.add('open');
    overlay.classList.add('active');
    document.body.style.overflow = 'hidden';
  }
  function closeSidebar() {
    sidebar.classList.remove('open');
    overlay.classList.remove('active');
    document.body.style.overflow = '';
  }
  burger && burger.addEventListener('click', openSidebar);
  overlay && overlay.addEventListener('click', closeSidebar);
  window.addEventListener('resize', function() {
    if(window.innerWidth >= 1200) closeSidebar();
  });
  // Закрытие по Esc
  document.addEventListener('keydown', function(e) {
    if(e.key === 'Escape') closeSidebar();
  });

  // --- Account dropdown logic (header) ---
  document.querySelectorAll('.header__account-dropdown').forEach(function(dropdown) {
    const btn = dropdown.querySelector('.header__account');
    const menu = dropdown.querySelector('.header__account-menu');
    if (btn && menu) {
      btn.addEventListener('click', function(e) {
        e.stopPropagation();
        menu.classList.toggle('open');
      });
      document.addEventListener('click', function(e) {
        if (!dropdown.contains(e.target)) {
          menu.classList.remove('open');
        }
      });
      // Переходы по кнопкам
      const signInBtn = menu.querySelector('.header__account-menu-btn:nth-child(1)');
      const signUpBtn = menu.querySelector('.header__account-menu-btn:nth-child(2)');
      if (signInBtn) {
        signInBtn.addEventListener('click', function() {
          window.location.href = 'auth.html';
        });
      }
      if (signUpBtn) {
        signUpBtn.addEventListener('click', function() {
          window.location.href = 'register.html';
        });
      }
    }
  });

  // --- Account dropdown logic (sidebar) ---
  document.querySelectorAll('.sidebar__account-dropdown').forEach(function(dropdown) {
    const btn = dropdown.querySelector('.sidebar__account');
    const menu = dropdown.querySelector('.sidebar__account-menu');
    if (btn && menu) {
      btn.addEventListener('click', function(e) {
        e.stopPropagation();
        menu.classList.toggle('open');
      });
      document.addEventListener('click', function(e) {
        if (!dropdown.contains(e.target)) {
          menu.classList.remove('open');
        }
      });
      // Переходы по кнопкам
      const signInBtn = menu.querySelector('.sidebar__account-menu-btn:nth-child(1)');
      const signUpBtn = menu.querySelector('.sidebar__account-menu-btn:nth-child(2)');
      if (signInBtn) {
        signInBtn.addEventListener('click', function() {
          window.location.href = 'auth.html';
        });
      }
      if (signUpBtn) {
        signUpBtn.addEventListener('click', function() {
          window.location.href = 'register.html';
        });
      }
    }
  });

  // --- Language Switcher Enhancement ---
  document.querySelectorAll('.lang-switch-btn').forEach(function(btn) {
    // Добавляем эффект нажатия
    btn.addEventListener('mousedown', function() {
      this.style.transform = 'translateY(-1px) scale(0.98)';
    });
    
    btn.addEventListener('mouseup', function() {
      this.style.transform = '';
    });
    
    btn.addEventListener('mouseleave', function() {
      this.style.transform = '';
    });

    // Добавляем эффект загрузки при отправке формы
    btn.addEventListener('click', function() {
      const form = this.closest('form');
      if (form) {
        // Добавляем класс загрузки
        this.classList.add('loading');
        
        // Убираем класс загрузки через небольшую задержку
        setTimeout(() => {
          this.classList.remove('loading');
        }, 1000);
      }
    });

    // Добавляем эффект при наведении
    btn.addEventListener('mouseenter', function() {
      this.style.zIndex = '10';
    });
    
    btn.addEventListener('mouseleave', function() {
      this.style.zIndex = '';
    });
  });

  // Анимация появления переключателя языков
  const languageSwitcher = document.querySelector('.sidebar__language-switcher-item');
  if (languageSwitcher) {
    // Добавляем небольшую задержку для анимации
    setTimeout(() => {
      languageSwitcher.style.opacity = '0';
      languageSwitcher.style.transform = 'translateY(20px)';
      languageSwitcher.style.transition = 'all 0.6s ease-out';
      
      setTimeout(() => {
        languageSwitcher.style.opacity = '1';
        languageSwitcher.style.transform = 'translateY(0)';
      }, 100);
    }, 200);
  }

  // Улучшенная анимация для кнопок смены языка
  document.querySelectorAll('.lang-switch-btn').forEach(function(btn) {
    btn.addEventListener('click', function() {
      // Добавляем класс для анимации изменения
      this.classList.add('changing');
      
      // Убираем класс через время анимации
      setTimeout(() => {
        this.classList.remove('changing');
      }, 500);
      
      // Добавляем эффект пульсации
      this.style.animation = 'activePulseEnhanced 0.5s ease-in-out';
      
      setTimeout(() => {
        this.style.animation = '';
      }, 500);
    });
  });

  // Анимация для заголовка переключателя языков
  const languageHeader = document.querySelector('.language-switcher-header');
  if (languageHeader) {
    // Добавляем анимацию появления для заголовка
    languageHeader.style.opacity = '0';
    languageHeader.style.transform = 'translateX(-20px)';
    languageHeader.style.transition = 'all 0.8s ease-out';
    
    setTimeout(() => {
      languageHeader.style.opacity = '1';
      languageHeader.style.transform = 'translateX(0)';
    }, 400);
  }
}); 